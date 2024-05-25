import socket
import time
from threading import Thread
import cv2
import HandTrackingModule as htm
import datetime

def receive_data(sock):
    global flag
    while True:
        data, addr = sock.recvfrom(1024)
        # print("Received message:", data.decode())

        if data.decode() == "Run":
            flag = True
        elif data.decode() == "Stop":
            flag = False


pTime = 0
cTime = 0
cap = cv2.VideoCapture(0)
detector = htm.handDetector()

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
sock.bind(('localhost', 12345))
flag = True
# flag = False
# recv_thread = Thread(target=receive_data, args=(sock,))
# recv_thread.start()

message = ""

while True:
    if flag:
        success, img = cap.read()
        img = detector.findHands(img)
        lmList = detector.findPosition(img)
        current_time = datetime.datetime.now().strftime("%Y-%m-%d %H:%M:%S.%f")
        # print(detector.detectIndexFingerFourDirections(lmList) + " | " , current_time)

        message = detector.detectIndexFingerFourDirections(lmList)
        # message = detector.detectIndexFingerFourDirections(lmList) + " " + current_time

        cTime = time.time()
        fps = 1 / (cTime - pTime)
        pTime = cTime

        # flp video
        img = cv2.flip(img, 1)

        cv2.putText(img, str(int(fps)), (10, 70), cv2.FONT_HERSHEY_PLAIN, 3, (255, 0, 255), 3)
        cv2.imshow("Image", img)
        cv2.waitKey(1)

        # send data to Unity
        sock.sendto(message.encode(), ('localhost', 54321))
