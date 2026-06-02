import type { ContactReadDto, ContactCreateDto, ContactUpdateDto } from "../types/ContactTypes";

const apiUrl = "/api/v1/contacts";

export async function fetchAllContacts(): Promise<ContactReadDto[]> {
    const response = await fetch(apiUrl);

    return await response.json();
}

export async function createContact(dto: ContactCreateDto): Promise<ContactReadDto> {
    const response = await fetch(apiUrl, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(dto)
    });

    await checkResponseForErrors(response);

    return await response.json();
}

export async function updateContact(id: number, dto: ContactUpdateDto): Promise<void> {
    const response = await fetch(`${apiUrl}/${id}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(dto)
    });

    await checkResponseForErrors(response);
}

export async function deleteContact(id: number): Promise<void> {
    const response = await fetch(`${apiUrl}/${id}`, {
        method: "DELETE"
    });

    await checkResponseForErrors(response);
}

async function checkResponseForErrors(response: Response): Promise<void> {
    if (!response.ok) {
        const errorData = await response.json();

        throw errorData;
    }
}