#include "scs.hpp"

#define PIN_SCS_IN 15
#define PIN_SCS_OUT 14

SCS scsbus(PIN_SCS_IN, PIN_SCS_OUT);
//uint8_t packetId = 0;
void scsReciveData(uint8_t* data, uint8_t size)
{
  //Serial.write(packetId++);
  Serial.write(size);
  Serial.write(data, size);
}

void setup()
{
  scsbus.onRecive(scsReciveData);
  scsbus.begin();

  Serial.begin();
  Serial.ignoreFlowControl(true);

  delay(1000); 
}

void loop()
{
  scsbus.update();
}
