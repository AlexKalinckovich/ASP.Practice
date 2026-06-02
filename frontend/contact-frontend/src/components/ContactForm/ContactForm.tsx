import { useState, useCallback, memo } from "react";
import type { ChangeEvent, SyntheticEvent } from "react";
import type { ContactCreateDto, ContactFormProps, ProblemDetails } from "../../types/ContactTypes";
import {
    validateContact,
    hasValidationErrors,
    getActiveErrors,
    hasGlobalError,
    getGlobalErrorTitle,
    getGlobalErrorDetail
} from "./contactFormUtils";
import styles from "./ContactForm.module.css";

interface GlobalErrorProps {
    apiError: ProblemDetails | null;
}

interface FieldErrorListProps {
    errors: string[];
}

interface FormFieldProps {
    id: string;
    name: string;
    label: string;
    type: string;
    value: string;
    onChange: (event: ChangeEvent<HTMLInputElement>) => void;
    errors: string[];
}

interface FormActionsProps {
    onCancel: () => void;
}

const GlobalError = memo(function GlobalError({ apiError }: GlobalErrorProps) {
    if (!hasGlobalError(apiError)) {
        return null;
    }

    return (
        <div className={styles.globalError}>
            <p className={styles.globalErrorText}>{getGlobalErrorTitle(apiError)}</p>
            <p className={styles.globalErrorText}>{getGlobalErrorDetail(apiError)}</p>
        </div>
    );
});

const FieldErrorList = memo(function FieldErrorList({ errors }: FieldErrorListProps) {
    if (errors.length === 0) {
        return null;
    }

    return (
        <div className={styles.errorContainer}>
            {errors.map((msg: string, index: number) => (
                <p key={index} className={styles.errorText}>{msg}</p>
            ))}
        </div>
    );
});

const FormField = memo(function FormField({ id, name, label, type, value, onChange, errors }: FormFieldProps) {
    const hasError: boolean = errors.length > 0;
    const inputClass: string = hasError ? `${styles.input} ${styles.inputError}` : styles.input;

    return (
        <div className={styles.formGroup}>
            <label htmlFor={id} className={styles.label}>{label}</label>
            <input
                type={type}
                id={id}
                name={name}
                value={value}
                onChange={onChange}
                className={inputClass}
            />
            <FieldErrorList errors={errors} />
        </div>
    );
});

const FormActions = memo(function FormActions({ onCancel }: FormActionsProps) {
    return (
        <div className={styles.buttonGroup}>
            <button type="button" onClick={onCancel} className={styles.cancelButton}>
                Cancel
            </button>
            <button type="submit" className={styles.submitButton}>
                Save
            </button>
        </div>
    );
});

const ContactForm = memo(function ContactForm({ initialData, apiError, onSubmit, onCancel }: ContactFormProps) {
    const [formData, setFormData] = useState<ContactCreateDto>({
        name: initialData?.name ?? "",
        mobilePhone: initialData?.mobilePhone ?? "",
        jobTitle: initialData?.jobTitle ?? "",
        birthDate: initialData?.birthDate ?? "",
    });

    const [localErrors, setLocalErrors] = useState<Record<string, string[]>>({});

    const handleInputChange = useCallback((event: ChangeEvent<HTMLInputElement>) => {
        const { name, value } = event.target;
        setFormData((previousData: ContactCreateDto) => ({ ...previousData, [name]: value }));
        setLocalErrors((previousErrors: Record<string, string[]>) => ({ ...previousErrors, [name]: [] }));
    }, []);

    const handleSubmit = useCallback((event: SyntheticEvent<HTMLFormElement>) => {
        event.preventDefault();
        const validationResult: Record<string, string[]> = validateContact(formData);

        if (hasValidationErrors(validationResult)) {
            setLocalErrors(validationResult);
            return;
        }

        onSubmit(formData);
    }, [formData, onSubmit]);

    return (
        <form onSubmit={handleSubmit} className={styles.form}>
            <GlobalError apiError={apiError} />

            <FormField
                id="name"
                name="name"
                label="Name"
                type="text"
                value={formData.name}
                onChange={handleInputChange}
                errors={getActiveErrors("name", "Name", localErrors, apiError)}
            />

            <FormField
                id="mobilePhone"
                name="mobilePhone"
                label="Mobile Phone"
                type="text"
                value={formData.mobilePhone}
                onChange={handleInputChange}
                errors={getActiveErrors("mobilePhone", "MobilePhone", localErrors, apiError)}
            />

            <FormField
                id="jobTitle"
                name="jobTitle"
                label="Job Title"
                type="text"
                value={formData.jobTitle}
                onChange={handleInputChange}
                errors={getActiveErrors("jobTitle", "JobTitle", localErrors, apiError)}
            />

            <FormField
                id="birthDate"
                name="birthDate"
                label="Birth Date"
                type="date"
                value={formData.birthDate}
                onChange={handleInputChange}
                errors={getActiveErrors("birthDate", "BirthDate", localErrors, apiError)}
            />

            <FormActions onCancel={onCancel} />
        </form>
    );
});

export default ContactForm;