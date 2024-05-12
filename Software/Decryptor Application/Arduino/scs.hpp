//SCS Legrand Bticino Interface
#include <stdint.h>
#ifndef SCS_H
#define SCS_H

#define SCS_DELAY_BETWEEN_2_EQUIPMENT_US 9580
#define SCS_DELAY_BETWEEN_2_REPETITION_US 3230

#define SCS_SEND_BUFFER_SIZE 11
#define SCS_RECIVE_BUFFER_SIZE 16

#define SCS_AUTOMATION_CMD_ON       0x00
#define SCS_AUTOMATION_CMD_OFF      0x01
#define SCS_AUTOMATION_CMD_DIM_INC  0x03
#define SCS_AUTOMATION_CMD_DIM_DEC  0x04
#define SCS_AUTOMATION_CMD_UP       0x08
#define SCS_AUTOMATION_CMD_DOWN     0x09
#define SCS_AUTOMATION_CMD_STOP     0x0A
#define SCS_AUTOMATION_CMD_DIM_10   0x0D
#define SCS_AUTOMATION_CMD_DIM_20   0x1D
#define SCS_AUTOMATION_CMD_DIM_30   0x2D
#define SCS_AUTOMATION_CMD_DIM_40   0x3D
#define SCS_AUTOMATION_CMD_DIM_50   0x4D
#define SCS_AUTOMATION_CMD_DIM_60   0x5D
#define SCS_AUTOMATION_CMD_DIM_70   0x6D
#define SCS_AUTOMATION_CMD_DIM_80   0x7D
#define SCS_AUTOMATION_CMD_DIM_90   0x8D

class SCS
{
  public:
  // Constructor
  SCS(pin_size_t _pinScsIn, pin_size_t _pinScsOut);

  // Functions
  void onRecive(void (*_func)(uint8_t* data, uint8_t size));
  void setDelays(uint32_t _scsOutDataDelay_RTW, uint32_t _scsOutDataDelay_WTW);
  
  uint8_t bufferSendAvailable();
 
  void markToSend(uint8_t _size);
  void markToSendDelayed(uint8_t _size);
  void markToSendDelayedWithCount(uint8_t _size, uint8_t _count);

  void sendACK();

  void begin();
  void update();

  // Vars
  uint8_t scsOutDataBuffer[SCS_SEND_BUFFER_SIZE];

  private:
  // Functions
  void(*funcOnRecive)(uint8_t* data, uint8_t size);
  void sendBit(uint8_t level);

  // Vars
  pin_size_t pinScsIn, pinScsOut;
  uint8_t scsInCurrentStatus;
  uint8_t scsInStatus;
  uint8_t scsInDataStatus;
  uint8_t scsInDataArrayStatus;
  uint8_t scsInData;
  uint8_t scsInDataErr;

  uint64_t scsInStartTime;
  uint64_t scsInTimeNow;
  uint64_t scsInElapsedTime;
  uint32_t scsInBitIndex;
  uint32_t scsInBitTime;

  uint8_t scsInDataBufferIndex;
  uint8_t scsInDataBuffer[SCS_RECIVE_BUFFER_SIZE];

  uint8_t scsOutDataBufferSize;
  uint8_t scsOutDataSendFlag;
  
  uint32_t scsOutDataDelay_RTW;
  uint32_t scsOutDataDelay_WTW;

  uint64_t scsInDataArrayEnd;
  uint8_t scsOutDataDelayType;

  uint8_t scsOutDataDelayedSize;
  uint8_t scsOutDataCount;

};

#endif