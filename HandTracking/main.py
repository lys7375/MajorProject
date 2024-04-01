# import cv2
# import threading
# import HandTrackingModule as htm
# import socket
# import numpy as np
# import queue
# import time
#
# frame_queue = queue.Queue()
#
#
# class FrameThread(threading.Thread):
#     def __init__(self):
#         super().__init__()
#         self.frame = None
#         self.go_on = True
#         self.pTime = 0
#
#     def run(self):
#         while self.go_on:
#             try:
#                 if self.frame is not None:
#                     # Copy the original frame
#                     display_frame = self.frame.copy()
#
#                     # 水平翻转视频
#                     display_frame = cv2.flip(display_frame, 1)
#
#                     cTime = time.time()
#                     fps = 1 / (cTime - self.pTime)
#                     self.pTime = cTime
#
#                     cv2.putText(display_frame, str(int(fps)), (10, 70), cv2.FONT_HERSHEY_PLAIN, 3, (255, 0, 255), 3)
#                     cv2.imshow("Frame", display_frame)
#
#                     if cv2.waitKey(1) & 0xFF == ord('q'):  # Press 'q' to quit
#                         self.go_on = False
#             except Exception as e:
#                 print(e)
#
#     def stop(self):
#         self.go_on = False
#
#
# def receive_and_send_data(frame_thread):
#     # 创建一个新的socket用来向Unity发送数据
#     sock_send = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
#     UDP_IP_SEND = "127.0.0.1"
#     UDP_PORT_SEND = 55555
#
#     sock_recv = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
#     UDP_IP_RECV = "0.0.0.0"
#     UDP_PORT_RECV = 8080
#     sock_recv.bind((UDP_IP_RECV, UDP_PORT_RECV))
#
#     detector = htm.handDetector()
#
#     while True:
#         data, addr = sock_recv.recvfrom(65535)
#         # 接受视频数据并处理
#         nparr = np.frombuffer(data, np.uint8)
#         frame = cv2.imdecode(nparr, cv2.IMREAD_COLOR)
#
#         if frame is not None:
#             # video frame processing
#             frame = detector.findHands(frame)
#             lmList = detector.findPosition(frame)
#
#             message = detector.detectIndexFingerFourDirections(lmList)
#             # print(detector.detectIndexFingerFourDirections(lmList))
#
#             print(message)
#
#             # message = ""
#             # for direction in detector.detectIndexFingerFourDirections(lmList):
#             #     message = direction
#             #     print(direction)
#
#             frame_thread.frame = frame  # 将frame赋值给FrameThread的实例
#
#             # 向Unity发送处理后的信息
#             sock_send.sendto(str(message).encode(), (UDP_IP_SEND, UDP_PORT_SEND))
#
#
# if __name__ == "__main__":
#     frame_thread = FrameThread()
#     frame_thread.start()
#
#     threading.Thread(target=receive_and_send_data, args=(frame_thread,)).start()

import socket
import time
from threading import Thread
import cv2
import time
import HandTrackingModule as htm


def receive_data(sock):
    global flag
    while True:
        data, addr = sock.recvfrom(1024) # 接收数据
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
# 启动接收数据的线程
recv_thread = Thread(target=receive_data, args=(sock,))
recv_thread.start()

message = ""

while True:
    if flag:
        success, img = cap.read()
        img = detector.findHands(img)
        lmList = detector.findPosition(img)
        # print(detector.detectIndexFingerFourDirections(lmList))

        message = detector.detectIndexFingerFourDirections(lmList)

        # cTime = time.time()
        # fps = 1 / (cTime - pTime)
        # pTime = cTime
        #
        # # 视频翻转
        # img = cv2.flip(img, 1)
        #
        # # 显示帧率
        # cv2.putText(img, str(int(fps)), (10, 70), cv2.FONT_HERSHEY_PLAIN, 3, (255, 0, 255), 3)
        # # 显示视频流
        # cv2.imshow("Image", img)
        # cv2.waitKey(1)

        # 发送数据到Unity
        sock.sendto(message.encode(), ('localhost', 54321))
