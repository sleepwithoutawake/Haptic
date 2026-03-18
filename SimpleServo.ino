// Simple code to change servomotor from Arduino
#include <Servo.h>  

Servo myServo;  // create servo object
int commandServo = 0; // change this to move the servo, goes from 0 degrees to 180 degrees

void setup()
{
  Serial.begin(9600); // begin serial communication
  myServo.attach(9); // servomotor on Pin 9 : TO CHECK 

  myServo.write(commandServo); // write on servomotor
}

void loop()
{  
  // do nothing 
}






