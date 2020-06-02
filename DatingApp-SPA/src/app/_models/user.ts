import { Photo } from './photo';

export interface User {
    // צריך לכתוב את כל המאפיינים שכתובים בתקיית DTO ןלכן המאפיינים שנמצאים רק במשתמש יחיד ומפורט נשים ?
    id: number;
    username: string;
    knowAs: string;
    age: number;
    gender: string;
    created: Date;
    lastActive: any;
    photoUrl: string;
    city: string;
    country: string;
    interests?: string;
    introduction?: string;
    lookingFor?: string;
    photos?: Photo[];
}
