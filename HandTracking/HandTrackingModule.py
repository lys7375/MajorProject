import math

import cv2
import mediapipe as mp


class handDetector():
    def __init__(self, mode=False, maxHands=2, complexity=1, detectionCon=0.5, trackCon=0.5):
        self.mode = mode
        self.maxHands = maxHands
        self.complexity = complexity
        self.detectionCon = detectionCon
        self.trackCon = trackCon

        # 导入mediapipe库中的手部检测模型
        self.mpHands = mp.solutions.hands
        # 创建模型实例
        self.hands = self.mpHands.Hands(self.mode, self.maxHands, self.complexity, self.detectionCon, self.trackCon)
        # 导入mediapipe库中的绘画工具
        self.mpDraw = mp.solutions.drawing_utils

    '''
    在图像中检测手部，并绘制手部关键点和连线

    参数:
        img: 传入的检测图像
        draw: 是否在图像上绘制手部关键点和连线
    '''

    def findHands(self, img, draw=True):
        # 将图像从BGR格式转换为mediapipe需要的RGB格式
        imgRGB = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
        self.results = self.hands.process(imgRGB)

        # 如果检测到手部则绘制手部关键点和关键点连线
        if self.results.multi_hand_landmarks:
            for handLms in self.results.multi_hand_landmarks:
                if draw:
                    self.mpDraw.draw_landmarks(img, handLms, self.mpHands.HAND_CONNECTIONS)
        return img

    # 获取指定手的关键点位置信息
    def findPosition(self, img, handNo=0, draw=True):

        lmList = []
        if self.results.multi_hand_landmarks:
            myHand = self.results.multi_hand_landmarks[handNo]
            for id, lm in enumerate(myHand.landmark):
                h, w, c = img.shape
                cx, cy = int(lm.x * w), int(lm.y * h)
                lmList.append([id, cx, cy])

        return lmList

    '''
    检测食指的四个基本方向（上、下、左、右）

    return:
        directions: 所有检测到的手的食指朝向
    '''

    # def detectIndexFingerFourDirections(self, img):
    #     directions = []
    #     if self.results.multi_hand_landmarks:
    #         for hand_landmarks in self.results.multi_hand_landmarks:
    #             # 获取食指第二个关节（index_finger_mcp）和第三个关节（index_finger_pip）的坐标
    #             x1 = hand_landmarks.landmark[self.mpHands.HandLandmark.INDEX_FINGER_MCP].x
    #             y1 = hand_landmarks.landmark[self.mpHands.HandLandmark.INDEX_FINGER_MCP].y
    #             x2 = hand_landmarks.landmark[self.mpHands.HandLandmark.INDEX_FINGER_PIP].x
    #             y2 = hand_landmarks.landmark[self.mpHands.HandLandmark.INDEX_FINGER_PIP].y
    #
    #             # 计算食指的斜率和角度（以度为单位）
    #             slope = (y2 - y1) / (x2 - x1)
    #             angle = math.atan(slope) * 180 / math.pi
    #
    #             # 根据角度判断食指的方向，并获取对应的反馈信息
    #             if (angle > 45 or angle < -45) and y1 > y2:
    #                 direction = "up"
    #             elif (angle > 45 or angle < -45) and y1 < y2:
    #                 direction = "down"
    #             elif (-45 < angle < 45) and x1 < x2:
    #                 direction = "left"
    #             else:
    #                 direction = "right"
    #
    #             directions.append(direction)
    #     return directions

    def detectIndexFingerFourDirections(self, img):
        left_direction = "NULL"
        right_direction = "NULL"

        if self.results.multi_hand_landmarks:
            for hand_landmarks, hand_handedness in zip(self.results.multi_hand_landmarks,
                                                       self.results.multi_handedness):
                direction = ""
                hand_label = hand_handedness.classification[0].label

                # 获取食指第二个关节（index_finger_mcp）和第三个关节（index_finger_pip）的坐标
                x1 = hand_landmarks.landmark[self.mpHands.HandLandmark.INDEX_FINGER_MCP].x
                y1 = hand_landmarks.landmark[self.mpHands.HandLandmark.INDEX_FINGER_MCP].y
                x2 = hand_landmarks.landmark[self.mpHands.HandLandmark.INDEX_FINGER_PIP].x
                y2 = hand_landmarks.landmark[self.mpHands.HandLandmark.INDEX_FINGER_PIP].y

                # 计算食指的斜率和角度（以度为单位）
                slope = (y2 - y1) / (x2 - x1)
                angle = math.atan(slope) * 180 / math.pi

                # 根据手的标签确定处理方向的逻辑
                if hand_label == 'Left':
                    if (angle > 45 or angle < -45) and y1 > y2:
                        right_direction = "Rup"
                    elif (angle > 45 or angle < -45) and y1 < y2:
                        right_direction = "Rdown"
                    elif (-45 < angle < 45) and x1 < x2:
                        right_direction = "Rleft"
                    else:
                        right_direction = "Rright"
                elif hand_label == 'Right':
                    if (angle > 45 or angle < -45) and y1 > y2:
                        left_direction = "Lup"
                    elif (angle > 45 or angle < -45) and y1 < y2:
                        left_direction = "Ldown"
                    elif (-45 < angle < 45) and x1 < x2:
                        left_direction = "Lleft"
                    else:
                        left_direction = "Lright"

        directions = f"{left_direction}|{right_direction}"

        return directions