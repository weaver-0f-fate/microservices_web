export interface Event {
    uuid: string,
    category: string,
    title: string,
    imageUrl: string,
    description: string,
    place: string,
    date: Date,
    additionalInfo: string,
    recurrency?: string,
}