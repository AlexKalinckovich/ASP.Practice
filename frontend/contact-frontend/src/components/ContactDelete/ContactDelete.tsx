import { memo } from "react";
import type { ContactReadDto, ProblemDetails } from "../../types/ContactTypes";
import {
    hasApiError,
    extractErrorTitle,
    extractErrorDetail,
    extractContactName
} from "./contactDeleteUtils";
import styles from "./ContactDelete.module.css";

interface ContactDeleteProps {
    contact: ContactReadDto;
    apiError: ProblemDetails | null;
    onConfirm: () => void;
    onCancel: () => void;
}

interface GlobalErrorProps {
    apiError: ProblemDetails | null;
}

interface WarningMessageProps {
    contact: ContactReadDto;
}

interface ActionButtonsProps {
    onConfirm: () => void;
    onCancel: () => void;
}

const GlobalError = memo(function GlobalError({ apiError }: GlobalErrorProps) {
    if (!hasApiError(apiError)) {
        return null;
    }

    return (
        <div className={styles.globalError}>
            <p className={styles.globalErrorText}>{extractErrorTitle(apiError)}</p>
            <p className={styles.globalErrorText}>{extractErrorDetail(apiError)}</p>
        </div>
    );
});

const WarningMessage = memo(function WarningMessage({ contact }: WarningMessageProps) {
    return (
        <p className={styles.warningText}>
            Are you sure you want to delete the contact{" "}
            <span className={styles.contactName}>{extractContactName(contact)}</span>
            ? This action cannot be undone.
        </p>
    );
});

const ActionButtons = memo(function ActionButtons({ onConfirm, onCancel }: ActionButtonsProps) {
    return (
        <div className={styles.buttonGroup}>
            <button type="button" onClick={onCancel} className={styles.cancelButton}>
                Cancel
            </button>
            <button type="button" onClick={onConfirm} className={styles.deleteButton}>
                Delete
            </button>
        </div>
    );
});

const ContactDelete = memo(function ContactDelete({ contact, apiError, onConfirm, onCancel }: ContactDeleteProps) {
    return (
        <div className={styles.container}>
            <GlobalError apiError={apiError} />
            <WarningMessage contact={contact} />
            <ActionButtons onConfirm={onConfirm} onCancel={onCancel} />
        </div>
    );
});

export default ContactDelete;