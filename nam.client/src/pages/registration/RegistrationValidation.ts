/**
 * Validation utility for the registration page.
 * Returns errors for each field and an `isValid` flag.
 */

export interface RegistrationData {
    email: string;
    password: string;
    confirmPassword: string;
}

export interface ValidationErrors {
    email?: string;
    password?: string;
    confirmPassword?: string;
}

/**
 * Check email format (simple but practical).
 */
export function isValidEmail(email: string): boolean {
    if (!email) {
        return false;
    }
    // Simple regex for email. Does not validate every RFC case but is sufficient for client-side UX.
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email.trim());
}

/**
 * Validates the password according to common rules:
 * - minimum 8 characters
 * - at least one lowercase letter
 * - at least one uppercase letter
 * - at least one digit
 * - at least one special character
 */
export function validatePassword(password: string): string | undefined {
    if (!password) {
        return 'Password is required.';
    }

    if (password.length < 8) {
        return 'Password must contain at least 8 characters.';
    }

    if (!/[a-z]/.test(password)) {
        return 'Password must contain at least one lowercase letter.';
    }

    if (!/[A-Z]/.test(password)) {
        return 'Password must contain at least one uppercase letter.';
    }

    if (!/[0-9]/.test(password)) {
        return 'Password must contain at least one digit.';
    }

    if (!/[^A-Za-z0-9]/.test(password)) {
        return 'Password must contain at least one special character.';
    }

    return undefined;
}

/**
 * Validates the confirm password field.
 */
export function validateConfirmPassword(password: string, confirmPassword: string): string | undefined {
    if (!confirmPassword) {
        return 'Confirm password is required.';
    }

    if (password !== confirmPassword) {
        return 'Passwords do not match.';
    }

    return undefined;
}

/**
 * Performs complete validation of registration data.
 */
export function validateRegistration(data: RegistrationData): { isValid: boolean; errors: ValidationErrors } {
    const errors: ValidationErrors = {};

    if (!data.email) {
        errors.email = 'Email is required.';
    } else if (!isValidEmail(data.email)) {
        errors.email = 'Invalid email format.';
    }

    const pwdError = validatePassword(data.password);
    if (pwdError) {
        errors.password = pwdError;
    }

    const confirmError = validateConfirmPassword(data.password, data.confirmPassword);
    if (confirmError) {
        errors.confirmPassword = confirmError;
    }

    const isValid = Object.keys(errors).length === 0;

    return { isValid, errors };
}

export default {
    isValidEmail,
    validatePassword,
    validateConfirmPassword,
    validateRegistration,
};