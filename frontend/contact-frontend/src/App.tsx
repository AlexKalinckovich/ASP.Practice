import { memo } from "react";
import type { ReactElement } from "react";
import type { ContactReadDto, ContactCreateDto, ProblemDetails } from "./types/ContactTypes";
import { useContactManager } from "./hooks/useContactManager";
import ContactList from "./components/ContactList/ContactList";
import ContactForm from "./components/ContactForm/ContactForm";
import ContactDetails from "./components/ContactDetails/ContactDetails";
import ContactDelete from "./components/ContactDelete/ContactDelete";
import Popup from "./components/Popup/Popup";
import styles from "./App.module.css";

interface AppHeaderProps {
    onCreateClick: () => void;
}

interface CreatePopupProps {
    isActive: boolean;
    apiError: ProblemDetails | null;
    onClose: () => void;
    onSubmit: (dto: ContactCreateDto) => Promise<void>;
}

interface EditPopupProps {
    isActive: boolean;
    contact: ContactReadDto | null;
    apiError: ProblemDetails | null;
    onClose: () => void;
    onSubmit: (dto: ContactCreateDto) => Promise<void>;
}

interface ViewPopupProps {
    isActive: boolean;
    contact: ContactReadDto | null;
    onClose: () => void;
}

interface DeletePopupProps {
    isActive: boolean;
    contact: ContactReadDto | null;
    apiError: ProblemDetails | null;
    onClose: () => void;
    onConfirm: () => Promise<void>;
}

const AppHeader = memo(function AppHeader({ onCreateClick }: AppHeaderProps): ReactElement {
    return (
        <div className={styles.header}>
            <h1 className={styles.title}>Contact Manager</h1>
            <button type="button" className={styles.createButton} onClick={onCreateClick}>
                Add New Contact
            </button>
        </div>
    );
});

const CreateContactPopup = memo(function CreateContactPopup({ isActive, apiError, onClose, onSubmit }: CreatePopupProps): ReactElement | null {
    if (!isActive) {
        return null;
    }

    return (
        <Popup title="Create Contact" onClose={onClose}>
            <ContactForm
                apiError={apiError}
                onSubmit={onSubmit}
                onCancel={onClose}
            />
        </Popup>
    );
});

const EditContactPopup = memo(function EditContactPopup({ isActive, contact, apiError, onClose, onSubmit }: EditPopupProps): ReactElement | null {
    if (!isActive || !contact) {
        return null;
    }

    return (
        <Popup title="Edit Contact" onClose={onClose}>
            <ContactForm
                initialData={contact}
                apiError={apiError}
                onSubmit={onSubmit}
                onCancel={onClose}
            />
        </Popup>
    );
});

const ViewContactPopup = memo(function ViewContactPopup({ isActive, contact, onClose }: ViewPopupProps): ReactElement | null {
    if (!isActive || !contact) {
        return null;
    }

    return (
        <Popup title="Contact Details" onClose={onClose}>
            <ContactDetails
                contact={contact}
                onClose={onClose}
            />
        </Popup>
    );
});

const DeleteContactPopup = memo(function DeleteContactPopup({ isActive, contact, apiError, onClose, onConfirm }: DeletePopupProps): ReactElement | null {
    if (!isActive || !contact) {
        return null;
    }

    return (
        <Popup title="Delete Contact" onClose={onClose}>
            <ContactDelete
                contact={contact}
                apiError={apiError}
                onConfirm={onConfirm}
                onCancel={onClose}
            />
        </Popup>
    );
});

export default function App(): ReactElement {
    const manager = useContactManager();

    return (
        <div className={styles.appContainer}>
            <AppHeader onCreateClick={manager.onOpenCreate} />

            <ContactList
                contacts={manager.contacts}
                onView={manager.onOpenView}
                onEdit={manager.onOpenEdit}
                onDelete={manager.onOpenDelete}
            />

            <CreateContactPopup
                isActive={manager.activePopup === "Create"}
                apiError={manager.apiError}
                onClose={manager.onClosePopup}
                onSubmit={manager.onCreateSubmit}
            />

            <EditContactPopup
                isActive={manager.activePopup === "Edit"}
                contact={manager.selectedContact}
                apiError={manager.apiError}
                onClose={manager.onClosePopup}
                onSubmit={manager.onUpdateSubmit}
            />

            <ViewContactPopup
                isActive={manager.activePopup === "View"}
                contact={manager.selectedContact}
                onClose={manager.onClosePopup}
            />

            <DeleteContactPopup
                isActive={manager.activePopup === "Delete"}
                contact={manager.selectedContact}
                apiError={manager.apiError}
                onClose={manager.onClosePopup}
                onConfirm={manager.onDeleteConfirm}
            />
        </div>
    );
}