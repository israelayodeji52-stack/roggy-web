const API_URL =
    process.env.NEXT_PUBLIC_API_URL ?? "https://localhost:7212";

type ApiRequestOptions = RequestInit & {
    token?: string;
};

export async function apiRequest<T>(
    endpoint: string,
    options: ApiRequestOptions = {},
): Promise<T> {
    const { token, ...requestOptions } = options;

    const headers = new Headers(requestOptions.headers);

    headers.set("Content-Type", "application/json");

    if (token) {
        headers.set("Authorization", `Bearer ${token}`);
    }

    const response = await fetch(`${API_URL}${endpoint}`, {
        ...requestOptions,
        headers,
    });

    if (!response.ok) {
        let message = `Request failed with status ${response.status}.`;

        try {
            const error = await response.json();

            if (typeof error?.message === "string") {
                message = error.message;
            }
        } catch {
            // Ignore JSON parsing errors and keep the default message.
        }

        throw new Error(message);
    }

    if (response.status === 204) {
        return undefined as T;
    }

    return response.json() as Promise<T>;
}

export async function apiGet<T>(
    endpoint: string,
    token?: string,
): Promise<T> {
    return apiRequest<T>(endpoint, {
        method: "GET",
        token,
    });
}

export async function apiPost<T>(
    endpoint: string,
    body?: unknown,
    token?: string,
): Promise<T> {
    return apiRequest<T>(endpoint, {
        method: "POST",
        body: body === undefined ? undefined : JSON.stringify(body),
        token,
    });
}

export async function apiPut<T>(
    endpoint: string,
    body?: unknown,
    token?: string,
): Promise<T> {
    return apiRequest<T>(endpoint, {
        method: "PUT",
        body: body === undefined ? undefined : JSON.stringify(body),
        token,
    });
}

export async function apiDelete<T>(
    endpoint: string,
    token?: string,
): Promise<T> {
    return apiRequest<T>(endpoint, {
        method: "DELETE",
        token,
    });
}