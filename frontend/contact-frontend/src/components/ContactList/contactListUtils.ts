import type { ContactReadDto } from "../../types/ContactTypes";

export function formatContactDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString();
}

export function hasContacts(contacts: ContactReadDto[]): boolean {
    return contacts.length > 0;
}