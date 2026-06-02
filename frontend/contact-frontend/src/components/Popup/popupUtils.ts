import type { MouseEvent as ReactMouseEvent } from "react";

export function isOverlayClick(event: ReactMouseEvent<HTMLDivElement>, currentTarget: EventTarget): boolean {
    return event.target === currentTarget;
}