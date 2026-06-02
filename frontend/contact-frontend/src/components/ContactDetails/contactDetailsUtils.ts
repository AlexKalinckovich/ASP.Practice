export function formatBirthDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString();
}