import type { ContactReadDto, ProblemDetails } from "../../types/ContactTypes";

export function hasApiError(apiError: ProblemDetails | null): boolean {
    return apiError !== null;
}

export function extractErrorTitle(apiError: ProblemDetails | null): string {
    if (apiError) {
        return apiError.title;
    }
    return "";
}

export function extractErrorDetail(apiError: ProblemDetails | null): string {
    if (apiError) {
        return apiError.detail;
    }
    return "";
}

export function extractContactName(contact: ContactReadDto): string {
    return contact.name;
}