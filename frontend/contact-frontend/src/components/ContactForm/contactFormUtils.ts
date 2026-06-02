import type { ContactCreateDto, ProblemDetails } from "../../types/ContactTypes";

export function validateRequired(value: string, message: string): string[] {
    if (!value || value.trim() === "") {
        return [message];
    }
    return [];
}

export function validateContact(data: ContactCreateDto): Record<string, string[]> {
    return {
        name: validateRequired(data.name, "Name is required."),
        mobilePhone: validateRequired(data.mobilePhone, "Mobile phone is required."),
        jobTitle: validateRequired(data.jobTitle, "Job title is required."),
        birthDate: validateRequired(data.birthDate, "Birth date is required.")
    };
}

export function hasValidationErrors(errors: Record<string, string[]>): boolean {
    const totalErrorsCount: number = errors.name.length + errors.mobilePhone.length + errors.jobTitle.length + errors.birthDate.length;
    return totalErrorsCount > 0;
}

export function extractApiFieldErrors(apiField: string, apiError: ProblemDetails | null): string[] {
    if (apiError && apiError.errors && apiError.errors[apiField]) {
        return apiError.errors[apiField];
    }
    return [];
}

export function getActiveErrors(
    localField: string,
    apiField: string,
    localErrors: Record<string, string[]>,
    apiError: ProblemDetails | null
): string[] {
    const localErrorList: string[] = localErrors[localField];

    if (localErrorList && localErrorList.length > 0) {
        return localErrorList;
    }

    return extractApiFieldErrors(apiField, apiError);
}

export function hasGlobalError(apiError: ProblemDetails | null): boolean {
    return apiError !== null && (!apiError.errors || Object.keys(apiError.errors).length === 0);
}

export function getGlobalErrorTitle(apiError: ProblemDetails | null): string {
    if (apiError) {
        return apiError.title;
    }
    return "";
}

export function getGlobalErrorDetail(apiError: ProblemDetails | null): string {
    if (apiError) {
        return apiError.detail;
    }
    return "";
}