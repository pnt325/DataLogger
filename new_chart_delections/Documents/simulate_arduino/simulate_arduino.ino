void setup() {
  Serial.begin(115200);
}

void loop() {
  float volt = random(210, 230);
  float current = random(5, 10);
  float wat = random(30, 50);

  Serial.println("volt:" + String(volt));
  Serial.println("current:" + String(current));
  Serial.println("wat:" + String(wat));

  delay(50);
}
