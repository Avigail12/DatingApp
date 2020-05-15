export interface Message {
    id: number;
    senderId: number;
    senderKnowAs: string;
    senderPhotoUrl: string;
    recipientId: number;
    recipientKnowAs: string;
    recipienPhotoUrl: string;
    content: string;
    isRead: boolean;
    dateRead: Date;
    messageSend: Date;
}
