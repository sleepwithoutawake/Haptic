#include <Servo.h>

// Unity sends messages in the format bXXXe, for example b90e
Servo myServo;

int index = 0;
char buf[8] = {0};
String dataUnity = "";
int dataUnityInt = 0;
bool isStreaming = false;

void setup()
{
  Serial.begin(9600);
  myServo.attach(9);
  myServo.write(0);
}

void loop()
{
  while (Serial.available() > 0)
  {
    char value = Serial.read();

    if (value == 'b')
    {
      isStreaming = true;
      dataUnity = "";
      memset(buf, 0, sizeof(buf));
      index = 0;
    }

    if (isStreaming && value != '\0' && index < sizeof(buf) - 1)
    {
      buf[index] = value;
      index++;
    }

    if (value == 'e' && isStreaming)
    {
      for (int i = 1; i < index - 1; i++)
      {
        dataUnity += String(buf[i]);
      }

      dataUnityInt = dataUnity.toInt();
      dataUnityInt = constrain(dataUnityInt, 0, 180);

      myServo.write(dataUnityInt);

      Serial.print("Data = ");
      Serial.println(dataUnityInt);

      isStreaming = false;
      memset(buf, 0, sizeof(buf));
      index = 0;
    }
  }
}
