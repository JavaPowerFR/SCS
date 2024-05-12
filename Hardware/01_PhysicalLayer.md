physical layer:

- Product used in this test:
  - 1 Power supply BTICINO 346020 [datasheet](https://assets.legrand.com/pim/NP-FT-GT/U2831J.pdf)
  - 1 Contact interface BTICINO 3477 [datasheet EN](https://assets.legrand.com/pim/NP-FT-GT/MQ00272-c-EN.pdf) [datasheet FR](https://assets.legrand.com/pim/NP-FT-GT/MQ00272-c-FR.pdf)
  - 1 Light dimmer BTICINO F418U2 [datasheet EN](https://assets.legrand.com/pim/NP-FT-GT/ST-00001620-EN.pdf) [datasheet FR](https://assets.legrand.com/pim/NP-FT-GT/ST-00001620-FR.pdf)
 
    schematics:
  ![img](https://github.com/JavaPowerFR/SCS/blob/main/Hardware/BUS_schematic.png)

    screenshot of the oscilloscope capture of the scs bus with no data (idle):
    ![img](https://github.com/JavaPowerFR/SCS/blob/main/Hardware/BUS_NO_DATA.png)

    screenshot of the oscilloscope capture of the scs bus with data transmission:
    ![img](https://github.com/JavaPowerFR/SCS/blob/main/Hardware/BUS_DATA.png)

- Conclusion:
  - Bus Voltage = 27,8V
  - The bus carry power and data in the same cable.
  - The data is transmitted by falling voltage on bus.
  - The equipments connected on bus don't care the polarity (power supply included).
