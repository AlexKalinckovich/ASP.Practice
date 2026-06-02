import { memo, useCallback } from "react";
import type { ReactNode, MouseEvent as ReactMouseEvent, ReactElement } from "react";
import { isOverlayClick } from "./popupUtils";
import styles from "./Popup.module.css";

interface PopupProps {
    title: string;
    children: ReactNode;
    onClose: () => void;
}

interface PopupOverlayProps {
    children: ReactNode;
    onClose: () => void;
}

interface PopupHeaderProps {
    title: string;
    onClose: () => void;
}

interface PopupContentProps {
    children: ReactNode;
}

const PopupContent = memo(function PopupContent({ children }: PopupContentProps): ReactElement {
    return (
        <div className={styles.content}>
            {children}
        </div>
    );
});

const PopupHeader = memo(function PopupHeader({ title, onClose }: PopupHeaderProps): ReactElement {
    return (
        <div className={styles.header}>
            <h2 className={styles.title}>{title}</h2>
            <button type="button" className={styles.closeButton} onClick={onClose} aria-label="Close">
                &times;
            </button>
        </div>
    );
});

const PopupOverlay = memo(function PopupOverlay({ children, onClose }: PopupOverlayProps): ReactElement {
    const handleOverlayClick = useCallback((event: ReactMouseEvent<HTMLDivElement>): void => {
        if (isOverlayClick(event, event.currentTarget)) {
            onClose();
        }
    }, [onClose]);

    return (
        <div className={styles.overlay} onClick={handleOverlayClick} role="presentation">
            <div className={styles.modal} role="dialog" aria-modal="true">
                {children}
            </div>
        </div>
    );
});

const Popup = memo(function Popup({ title, children, onClose }: PopupProps): ReactElement {
    return (
        <PopupOverlay onClose={onClose}>
            <PopupHeader title={title} onClose={onClose} />
            <PopupContent>
                {children}
            </PopupContent>
        </PopupOverlay>
    );
});

export default Popup;