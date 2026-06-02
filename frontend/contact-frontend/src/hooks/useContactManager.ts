import { useState, useEffect, useCallback } from "react";
import type { ContactReadDto, ContactCreateDto, ContactUpdateDto, ProblemDetails } from "../types/ContactTypes";
import { fetchAllContacts, createContact, updateContact, deleteContact } from "../api/ContactApiClient";

export type PopupState = "None" | "Create" | "Edit" | "View" | "Delete";

export interface ContactManagerResult {
    contacts: ContactReadDto[];
    activePopup: PopupState;
    selectedContact: ContactReadDto | null;
    apiError: ProblemDetails | null;
    onOpenCreate: () => void;
    onOpenView: (contact: ContactReadDto) => void;
    onOpenEdit: (contact: ContactReadDto) => void;
    onOpenDelete: (contact: ContactReadDto) => void;
    onClosePopup: () => void;
    onCreateSubmit: (dto: ContactCreateDto) => Promise<void>;
    onUpdateSubmit: (dto: ContactCreateDto) => Promise<void>;
    onDeleteConfirm: () => Promise<void>;
}

export function useContactManager(): ContactManagerResult {
    const [contacts, setContacts] = useState<ContactReadDto[]>([]);
    const [activePopup, setActivePopup] = useState<PopupState>("None");
    const [selectedContact, setSelectedContact] = useState<ContactReadDto | null>(null);
    const [apiError, setApiError] = useState<ProblemDetails | null>(null);

    const loadContacts = useCallback(async (): Promise<void> => {
        try {
            const fetchedContacts: ContactReadDto[] = await fetchAllContacts();
            setContacts(fetchedContacts);
        } catch (error) {
            console.error(error);
        }
    }, []);

    useEffect(() => {
        void loadContacts();
    }, [loadContacts]);

    const handleOpenCreate = useCallback((): void => {
        setApiError(null);
        setSelectedContact(null);
        setActivePopup("Create");
    }, []);

    const handleOpenView = useCallback((contact: ContactReadDto): void => {
        setApiError(null);
        setSelectedContact(contact);
        setActivePopup("View");
    }, []);

    const handleOpenEdit = useCallback((contact: ContactReadDto): void => {
        setApiError(null);
        setSelectedContact(contact);
        setActivePopup("Edit");
    }, []);

    const handleOpenDelete = useCallback((contact: ContactReadDto): void => {
        setApiError(null);
        setSelectedContact(contact);
        setActivePopup("Delete");
    }, []);

    const handleClosePopup = useCallback((): void => {
        setActivePopup("None");
        setSelectedContact(null);
        setApiError(null);
    }, []);

    const handleCreateSubmit = useCallback(async (dto: ContactCreateDto): Promise<void> => {
        try {
            await createContact(dto);
            await loadContacts();
            handleClosePopup();
        } catch (error) {
            setApiError(error as ProblemDetails);
        }
    }, [handleClosePopup, loadContacts]);

    const handleUpdateSubmit = useCallback(async (dto: ContactCreateDto): Promise<void> => {
        if (!selectedContact) {
            return;
        }

        try {
            const updateDto: ContactUpdateDto = {
                id: selectedContact.id,
                name: dto.name,
                mobilePhone: dto.mobilePhone,
                jobTitle: dto.jobTitle,
                birthDate: dto.birthDate
            };

            await updateContact(selectedContact.id, updateDto);
            await loadContacts();
            handleClosePopup();
        } catch (error) {
            setApiError(error as ProblemDetails);
        }
    }, [selectedContact, handleClosePopup, loadContacts]);

    const handleDeleteConfirm = useCallback(async (): Promise<void> => {
        if (!selectedContact) {
            return;
        }

        try {
            await deleteContact(selectedContact.id);
            await loadContacts();
            handleClosePopup();
        } catch (error) {
            setApiError(error as ProblemDetails);
        }
    }, [selectedContact, handleClosePopup, loadContacts]);

    return {
        contacts,
        activePopup,
        selectedContact,
        apiError,
        onOpenCreate: handleOpenCreate,
        onOpenView: handleOpenView,
        onOpenEdit: handleOpenEdit,
        onOpenDelete: handleOpenDelete,
        onClosePopup: handleClosePopup,
        onCreateSubmit: handleCreateSubmit,
        onUpdateSubmit: handleUpdateSubmit,
        onDeleteConfirm: handleDeleteConfirm
    };
}