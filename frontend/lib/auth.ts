export type AuthUser = {
    id: string;
    username: string;
    email: string;
    displayName: string;
    profileImageUrl: string | null;
    bio: string | null;
    role: string;
    isActive: boolean;
};

export type LoginResponse = {
    userId: string;
    username: string;
    email: string;
    displayName: string;
    token: string;
};

const TOKEN_KEY = "roggy_token";
const USER_KEY = "roggy_user";

export function saveAuth(
    response: LoginResponse,
    user: AuthUser,
) {
    localStorage.setItem(TOKEN_KEY, response.token);
    localStorage.setItem(USER_KEY, JSON.stringify(user));
}

export function getToken(): string | null {
    if (typeof window === "undefined") {
        return null;
    }

    return localStorage.getItem(TOKEN_KEY);
}

export function getStoredUser(): AuthUser | null {
    if (typeof window === "undefined") {
        return null;
    }

    const storedUser = localStorage.getItem(USER_KEY);

    if (!storedUser) {
        return null;
    }

    try {
        return JSON.parse(storedUser) as AuthUser;
    } catch {
        localStorage.removeItem(USER_KEY);
        return null;
    }
}

export function clearAuth() {
    if (typeof window === "undefined") {
        return;
    }

    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
}