export interface ContactReadDto {
    id: number;
    name: string;
    mobilePhone: string;
    jobTitle: string;
    birthDate: string;
}

export interface ContactCreateDto {
    name: string;
    mobilePhone: string;
    jobTitle: string;
    birthDate: string;
}

export interface ContactUpdateDto {
    id: number;
    name: string;
    mobilePhone: string;
    jobTitle: string;
    birthDate: string;
}

export interface ProblemDetails {
    type: string;
    title: string;
    status: number;
    detail: string;
    instance: string;
    errors?: Record<string, string[]>;
}

export interface ContactFormProps {
    initialData?: ContactCreateDto;
    apiError: ProblemDetails | null;
    onSubmit: (data: ContactCreateDto) => void;
    onCancel: () => void;
}