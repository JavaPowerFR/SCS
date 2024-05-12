#include <Arduino.h>
#include "scs.hpp"

//#define SCS_DEBUG_SHOW_DATA_READ_TIME 13
//#define SCS_DEBUG_SHOW_MESSAGE_READ_TIME 12

SCS::SCS(pin_size_t _pinScsIn, pin_size_t _pinScsOut)
{
  pinScsIn = _pinScsIn;
  pinScsOut = _pinScsOut;

  scsInCurrentStatus = 0;
  scsInStatus = 0;
  scsInDataStatus = 0;
  scsInDataArrayStatus = 0;
  scsInData = 0;
  scsInDataErr = 0;

  scsInStartTime = 0;
  scsInTimeNow = 0;
  scsInElapsedTime = 0;
  scsInBitIndex = 0;
  scsInBitTime = 0;

  scsInDataBufferIndex = 0;
  memset(scsInDataBuffer, SCS_RECIVE_BUFFER_SIZE, 0);

  scsOutDataBufferSize = 0;
  scsOutDataSendFlag = 0;
  memset(scsOutDataBuffer, SCS_SEND_BUFFER_SIZE, 0);

  scsOutDataDelay_RTW = SCS_DELAY_BETWEEN_2_EQUIPMENT_US;
  scsOutDataDelay_WTW = SCS_DELAY_BETWEEN_2_REPETITION_US;

  scsOutDataDelayedSize = 0;
  scsOutDataCount = 0;

  scsInDataArrayEnd = 0;

  scsOutDataDelayType = 1;
}

void SCS::setDelays(uint32_t _scsOutDataDelay_RTW, uint32_t _scsOutDataDelay_WTW)
{
  scsOutDataDelay_RTW = _scsOutDataDelay_RTW;
  scsOutDataDelay_WTW = _scsOutDataDelay_WTW;
}

uint8_t SCS::bufferSendAvailable()
{
  return !scsOutDataSendFlag && scsOutDataCount == 0;
}

void SCS::markToSend(uint8_t _size)
{
  scsOutDataBufferSize = _size;
  scsOutDataSendFlag = 1;
}

void SCS::markToSendDelayed(uint8_t _size)
{
  scsOutDataDelayedSize = _size;
  scsOutDataCount = 1;
}

void SCS::markToSendDelayedWithCount(uint8_t _size, uint8_t _count)
{
  scsOutDataDelayedSize = _size;
  scsOutDataCount = _count;
}

void SCS::sendACK()
{
  if(bufferSendAvailable())
  {
    scsOutDataBuffer[0] = 0xA5;
    scsOutDataBufferSize = 1;
    scsOutDataSendFlag = 1;
  }
}

void SCS::onRecive(void (*_func)(uint8_t* data, uint8_t size))
{
  funcOnRecive = _func;
}

void SCS::begin()
{
  pinMode(pinScsIn, INPUT);
  pinMode(pinScsOut, OUTPUT);

  sio_hw->gpio_clr = (1 << pinScsOut);

  //DEBUG OPTIONS
  #ifdef SCS_DEBUG_SHOW_DATA_READ_TIME
  pinMode(SCS_DEBUG_SHOW_DATA_READ_TIME, OUTPUT);
  sio_hw->gpio_clr = (1 << SCS_DEBUG_SHOW_DATA_READ_TIME);
  #endif

  #ifdef SCS_DEBUG_SHOW_MESSAGE_READ_TIME
  pinMode(SCS_DEBUG_SHOW_MESSAGE_READ_TIME, OUTPUT);
  sio_hw->gpio_clr = (1 << SCS_DEBUG_SHOW_MESSAGE_READ_TIME);
  #endif

  scsInDataArrayEnd = time_us_64();
}

void SCS::sendBit(uint8_t level)
{
  if(level)
  {
    delayMicroseconds(104);
  }
  else
  {
    sio_hw->gpio_set = (1 << pinScsOut);
    delayMicroseconds(29);
    sio_hw->gpio_clr = (1 << pinScsOut);
    delayMicroseconds(75); // (104 - 29)
  }
}

void SCS::update()
{
  scsInCurrentStatus = digitalRead(pinScsIn);
  if(scsInDataStatus) // Data In Inprogress
  {
    scsInTimeNow = time_us_64();
    if(scsInTimeNow < scsInStartTime)
    {
      scsInElapsedTime = (0xFFFFFFFFFFFFFFFF - scsInStartTime) + scsInTimeNow;
    }
    else
    {
      scsInElapsedTime = scsInTimeNow - scsInStartTime;
    }

    scsInBitIndex = scsInElapsedTime / 104;
    scsInBitTime = scsInElapsedTime % 104;

    if(scsInBitTime > 60 && scsInBitIndex > 8)
    {
      if(!scsInDataErr)
      {
        scsInDataBuffer[scsInDataBufferIndex] = ~scsInData;
        ++scsInDataBufferIndex;
      }
      
      // DEBUG
      #ifdef SCS_DEBUG_SHOW_DATA_READ_TIME
      sio_hw->gpio_clr = (1 << SCS_DEBUG_SHOW_DATA_READ_TIME);
      #endif

      scsInDataStatus = 0;
    }
    
    if(scsInElapsedTime > 1041) //TimeOut
    {
      // DEBUG
      #ifdef SCS_DEBUG_SHOW_DATA_READ_TIME
      sio_hw->gpio_clr = (1 << SCS_DEBUG_SHOW_DATA_READ_TIME);
      #endif

      scsInDataStatus = 0;
    }
  }
  else if(scsInDataArrayStatus) // Data In Array Check Timeout
  {
    //Check end of the array
    scsInTimeNow = time_us_64();
    if(scsInTimeNow < scsInStartTime)
    {
      scsInElapsedTime = (0xFFFFFFFFFFFFFFFF - scsInStartTime) + scsInTimeNow;
    }
    else
    {
      scsInElapsedTime = scsInTimeNow - scsInStartTime;
    }

    if(scsInElapsedTime > 1170) //TimeOut (104*10) + 130
    {
      scsInDataArrayStatus = 0;

      funcOnRecive(scsInDataBuffer, scsInDataBufferIndex);
      scsInDataArrayEnd = scsInTimeNow;
      scsOutDataDelayType = 1;

      #ifdef SCS_DEBUG_SHOW_MESSAGE_READ_TIME
      sio_hw->gpio_clr = (1 << SCS_DEBUG_SHOW_MESSAGE_READ_TIME);
      #endif

    }

  }
  else if(scsOutDataSendFlag) // Data Out
  {
    //SEND
    for(size_t i = 0; i < scsOutDataBufferSize; ++i)
    {
      sendBit(0); // start
      sendBit(scsOutDataBuffer[i] & 1);
      sendBit(scsOutDataBuffer[i] & 2);
      sendBit(scsOutDataBuffer[i] & 4);
      sendBit(scsOutDataBuffer[i] & 8);
      sendBit(scsOutDataBuffer[i] & 16);
      sendBit(scsOutDataBuffer[i] & 32);
      sendBit(scsOutDataBuffer[i] & 64);
      sendBit(scsOutDataBuffer[i] & 128);
      sendBit(1); // stop
      delayMicroseconds(34);
    }

    scsOutDataSendFlag = 0;
    scsOutDataBufferSize = 0;
    scsInDataArrayEnd = time_us_64();

    return;
  }
  else if(scsOutDataCount) // Check data out (delayed)
  {
    scsInTimeNow = time_us_64();
    if(scsInTimeNow < scsInDataArrayEnd)
    {
      scsInElapsedTime = (0xFFFFFFFFFFFFFFFF - scsInDataArrayEnd) + scsInTimeNow;
    }
    else
    {
      scsInElapsedTime = scsInTimeNow - scsInDataArrayEnd;
    }

    if(scsOutDataDelayType) // DELAY BETWEEN 2 EQUIPMENT
    {
      if(scsInElapsedTime > scsOutDataDelay_RTW)
      {
        scsOutDataSendFlag = 1;
        scsOutDataBufferSize = scsOutDataDelayedSize;
        --scsOutDataCount;
        scsOutDataDelayType = 0;
      }
    }
    else // DELAY BETWEEN 2 REPETITION
    {
      if(scsInElapsedTime > scsOutDataDelay_WTW)
      {
        scsOutDataSendFlag = 1;
        scsOutDataBufferSize = scsOutDataDelayedSize;
        --scsOutDataCount;
      }
    }
  }

  if(scsInCurrentStatus != scsInStatus) // Check Data In Level
  {
    scsInStatus = scsInCurrentStatus;

    if(scsInCurrentStatus) // Rising Edge
    {
      if(scsInDataStatus == 0)
      {
        // Start Bit
        scsInData = 0;
        scsInDataErr = 0;

        scsInDataStatus = 1;

        if(scsInDataArrayStatus == 0)
          scsInDataBufferIndex = 0;
        scsInDataArrayStatus = 1;

        scsInStartTime = time_us_64();
        
        // DEBUG
        #ifdef SCS_DEBUG_SHOW_DATA_READ_TIME
        sio_hw->gpio_set = (1 << SCS_DEBUG_SHOW_DATA_READ_TIME);
        #endif

        #ifdef SCS_DEBUG_SHOW_MESSAGE_READ_TIME
        sio_hw->gpio_set = (1 << SCS_DEBUG_SHOW_MESSAGE_READ_TIME);
        #endif
      }
    }
    else // Falling Edge
    {
      if(scsInDataStatus == 1)
      {
        // Data In
        if(scsInBitTime < 101 && scsInBitIndex > 0)
        {
          if(scsInBitIndex < 9)
          {
            scsInData |= 1 << (scsInBitIndex-1);
          }
          else
          {
            scsInDataErr = 1;
          }
        }
      }
    }
  }
}