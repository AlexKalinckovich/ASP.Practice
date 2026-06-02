import { memo, useCallback } from "react";
import type { ReactElement } from "react";
import type { ContactReadDto } from "../../types/ContactTypes";
import { formatContactDate, hasContacts } from "./contactListUtils";
import styles from "./ContactList.module.css";

interface ContactListProps {
    contacts: ContactReadDto[];
    onView: (contact: ContactReadDto) => void;
    onEdit: (contact: ContactReadDto) => void;
    onDelete: (contact: ContactReadDto) => void;
}

interface ContactRowProps {
    contact: ContactReadDto;
    onView: (contact: ContactReadDto) => void;
    onEdit: (contact: ContactReadDto) => void;
    onDelete: (contact: ContactReadDto) => void;
}

interface EmptyStateProps {
    contacts: ContactReadDto[];
}

interface ContactTableProps {
    contacts: ContactReadDto[];
    onView: (contact: ContactReadDto) => void;
    onEdit: (contact: ContactReadDto) => void;
    onDelete: (contact: ContactReadDto) => void;
}

const EmptyState = memo(function EmptyState({ contacts }: EmptyStateProps): ReactElement | null {
    if (hasContacts(contacts)) {
        return null;
    }

    return (
        <div className={styles.emptyMessage}>
            <p>No contacts found. Please create one to get started.</p>
        </div>
    );
});

const TableHead = memo(function TableHead(): ReactElement {
    return (
        <thead>
        <tr>
            <th>Name</th>
            <th>Mobile Phone</th>
            <th>Job Title</th>
            <th>Birth Date</th>
            <th>Actions</th>
        </tr>
        </thead>
    );
});

const ContactRow = memo(function ContactRow({ contact, onView, onEdit, onDelete }: ContactRowProps): ReactElement {
    const handleViewAction = useCallback((): void => {
        onView(contact);
    }, [contact, onView]);

    const handleEditAction = useCallback((): void => {
        onEdit(contact);
    }, [contact, onEdit]);

    const handleDeleteAction = useCallback((): void => {
        onDelete(contact);
    }, [contact, onDelete]);

    return (
        <tr>
            <td>{contact.name}</td>
            <td>{contact.mobilePhone}</td>
            <td>{contact.jobTitle}</td>
            <td>{formatContactDate(contact.birthDate)}</td>
            <td className={styles.actions}>
                <button type="button" className={styles.viewButton} onClick={handleViewAction}>View</button>
                <button type="button" className={styles.editButton} onClick={handleEditAction}>Edit</button>
                <button type="button" className={styles.deleteButton} onClick={handleDeleteAction}>Delete</button>
            </td>
        </tr>
    );
});

const ContactTable = memo(function ContactTable({ contacts, onView, onEdit, onDelete }: ContactTableProps): ReactElement | null {
    if (!hasContacts(contacts)) {
        return null;
    }

    return (
        <table className={styles.table}>
            <TableHead />
            <tbody>
            {contacts.map((contact: ContactReadDto) => (
                <ContactRow
                    key={contact.id}
                    contact={contact}
                    onView={onView}
                    onEdit={onEdit}
                    onDelete={onDelete}
                />
            ))}
            </tbody>
        </table>
    );
});

const ContactList = memo(function ContactList({ contacts, onView, onEdit, onDelete }: ContactListProps): ReactElement {
    return (
        <div className={styles.tableContainer}>
            <EmptyState contacts={contacts} />
            <ContactTable
                contacts={contacts}
                onView={onView}
                onEdit={onEdit}
                onDelete={onDelete}
            />
        </div>
    );
});

export default ContactList;