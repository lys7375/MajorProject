import cv2
import time
import HandTrackingModule as htm
from flask import Flask, request, Response
from flask_socketio import SocketIO

# pTime = 0
# cTime = 0
# cap = cv2.VideoCapture(0)
# detector = htm.handDetector()
#
# while True:
#     success, img = cap.read()
#     img = detector.findHands(img)
#     lmList = detector.findPosition(img)
#     # if len(lmList) != 0:
#     #     print(lmList[4])
#     print(detector.detectIndexFingerFourDirections(lmList))
#
#     cTime = time.time()
#     fps = 1 / (cTime - pTime)
#     pTime = cTime
#
#     cv2.putText(img, str(int(fps)), (10, 70), cv2.FONT_HERSHEY_PLAIN, 3, (255, 0, 255), 3)
#
#     cv2.imshow("Image", img)
#     cv2.waitKey(1)

import socket
import numpy as np
import cv2

# UDP服务器设置
UDP_IP = "0.0.0.0"  # 监听所有来自的IP
UDP_PORT = 8080     # 设定端口号

sock = socket.socket(socket.AF_INET,  # Internet
                     socket.SOCK_DGRAM)  # UDP
sock.bind((UDP_IP, UDP_PORT))

while True:
    data, addr = sock.recvfrom(65535)  # 缓冲大小设置为65k
    nparr = np.frombuffer(data, np.uint8)
    frame = cv2.imdecode(nparr, cv2.IMREAD_COLOR)  # 将接收到的数据转换为OpenCV图像

    if frame is not None:
        cv2.imshow("Frame", frame)
        if cv2.waitKey(1) & 0xFF == ord('q'):  # 按'q'退出
            break

cv2.destroyAllWindows()