export default interface Event {
  eventId: number;
  eventName: string;
  description: string;
  dateTime: string;
  location: string;
  link: string | null;
}
