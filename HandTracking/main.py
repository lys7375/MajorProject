# import cv2
# import time
# import HandTrackingModule as htm
# from flask import Flask, request, Response
# from flask_socketio import SocketIO
#
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

# import socket
# import numpy as np
# import cv2
#
# UDP_IP = "0.0.0.0"
# UDP_PORT = 8080
#
# sock = socket.socket(socket.AF_INET,
#                      socket.SOCK_DGRAM)
# sock.bind((UDP_IP, UDP_PORT))
#
# while True:
#     data, addr = sock.recvfrom(65535)
#     nparr = np.frombuffer(data, np.uint8)
#     frame = cv2.imdecode(nparr, cv2.IMREAD_COLOR)
#
#     if frame is not None:
#         cv2.imshow("Frame", frame)
#         if cv2.waitKey(1) & 0xFF == ord('q'):
#             break
#
# cv2.destroyAllWindows()

# import cv2
# import threading
# import HandTrackingModule as htm
# import socket
# import numpy as np
# import queue
# import time
#
# # 创建一个队列用于线程通信
# frame_queue = queue.Queue()
#
# def receive_and_send_data():
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
#     while True:
#         data, addr = sock_recv.recvfrom(65535)
#         # 接受视频数据并处理
#         nparr = np.frombuffer(data, np.uint8)
#         frame = cv2.imdecode(nparr, cv2.IMREAD_COLOR)
#
#         if frame is not None:
#             frame_queue.put(frame)  # 将frame放入队列
#             message = "Only message"
#
#             # 向Unity发送处理后的信息
#             sock_send.sendto(str(message).encode(), (UDP_IP_SEND, UDP_PORT_SEND))
#
# def process_and_show_data():
#     pTime = 0
#     detector = htm.handDetector()
#
#     while True:
#         frame = frame_queue.get()  # 从队列中获取frame
#
#         if frame is not None:
#             # video frame processing
#             frame = detector.findHands(frame)
#             lmList = detector.findPosition(frame)
#             message = detector.detectIndexFingerFourDirections(lmList)
#
#             cTime = time.time()
#             fps = 1 / (cTime - pTime)
#             pTime = cTime
#
#             cv2.putText(frame, str(int(fps)), (10, 70), cv2.FONT_HERSHEY_PLAIN, 3, (255, 0, 255), 3)
#             cv2.imshow("Frame", frame)
#
#             if cv2.waitKey(1) & 0xFF == ord('q'):
#                 break
#
#     cv2.destroyAllWindows()
#
#
# # 创建并开启两个线程
# threading.Thread(target=receive_and_send_data).start()
# threading.Thread(target=process_and_show_data).start()

import cv2
import threading
import HandTrackingModule as htm
import socket
import numpy as np
import queue
import time

frame_queue = queue.Queue()

class FrameThread(threading.Thread):
    def __init__(self):
        super().__init__()
        self.frame = None
        self.go_on = True
        self.pTime = 0

    def run(self):
        while self.go_on:
            try:
                if self.frame is not None:
                    # Copy the original frame
                    display_frame = self.frame.copy()

                    cTime = time.time()
                    fps = 1 / (cTime - self.pTime)
                    self.pTime = cTime

                    cv2.putText(display_frame, str(int(fps)), (10, 70), cv2.FONT_HERSHEY_PLAIN, 3, (255, 0, 255), 3)
                    cv2.imshow("Frame", display_frame)

                    if cv2.waitKey(1) & 0xFF == ord('q'):  # Press 'q' to quit
                        self.go_on = False
            except Exception as e:
                print(e)

    def stop(self):
        self.go_on = False

def receive_and_send_data(frame_thread):
    # 创建一个新的socket用来向Unity发送数据
    sock_send = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    UDP_IP_SEND = "127.0.0.1"
    UDP_PORT_SEND = 55555

    sock_recv = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    UDP_IP_RECV = "0.0.0.0"
    UDP_PORT_RECV = 8080
    sock_recv.bind((UDP_IP_RECV, UDP_PORT_RECV))

    detector = htm.handDetector()

    while True:
        data, addr = sock_recv.recvfrom(65535)
        # 接受视频数据并处理
        nparr = np.frombuffer(data, np.uint8)
        frame = cv2.imdecode(nparr, cv2.IMREAD_COLOR)

        if frame is not None:
            # video frame processing
            frame = detector.findHands(frame)
            lmList = detector.findPosition(frame)
            message = detector.detectIndexFingerFourDirections(lmList)

            frame_thread.frame = frame  # 将frame赋值给FrameThread的实例

            # 向Unity发送处理后的信息
            sock_send.sendto(str(message).encode(), (UDP_IP_SEND, UDP_PORT_SEND))

if __name__ == "__main__":
    frame_thread = FrameThread()
    frame_thread.start()

    threading.Thread(target=receive_and_send_data, args=(frame_thread,)).start()