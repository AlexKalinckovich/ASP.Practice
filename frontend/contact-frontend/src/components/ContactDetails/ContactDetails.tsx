import { memo } from "react";
import type { ContactReadDto } from "../../types/ContactTypes";
import { formatBirthDate } from "./contactDetailsUtils";
import styles from "./ContactDetails.module.css";

interface ContactDetailsProps {
    contact: ContactReadDto;
    onClose: () => void;
}

interface DetailRowProps {
    label: string;
    value: string | number;
}

interface CloseButtonProps {
    onClose: () => void;
}

interface ContactInformationProps {
    contact: ContactReadDto;
}

const DetailRow = memo(function DetailRow({ label, value }: DetailRowProps) {
    return (
        <div className={styles.detailRow}>
            <span className={styles.label}>{label}</span>
            <p className={styles.value}>{value}</p>
        </div>
    );
});

const CloseButton = memo(function CloseButton({ onClose }: CloseButtonProps) {
    return (
        <button type="button" className={styles.closeButton} onClick={onClose}>
            Close
        </button>
    );
});

const ContactInformation = memo(function ContactInformation({ contact }: ContactInformationProps) {
    return (
        <>
            <DetailRow label="ID" value={contact.id} />
            <DetailRow label="Name" value={contact.name} />
            <DetailRow label="Mobile Phone" value={contact.mobilePhone} />
            <DetailRow label="Job Title" value={contact.jobTitle} />
            <DetailRow label="Birth Date" value={formatBirthDate(contact.birthDate)} />
        </>
    );
});

const ContactDetails = memo(function ContactDetails({ contact, onClose }: ContactDetailsProps) {
    return (
        <div className={styles.container}>
            <ContactInformation contact={contact} />
            <CloseButton onClose={onClose} />
        </div>
    );
});

export default ContactDetails;