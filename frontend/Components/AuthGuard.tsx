"use client";

import { ReactNode, useEffect } from "react";
import { usePathname, useRouter } from "next/navigation";

import { getToken } from "../lib/auth";

type AuthGuardProps = {
    children: ReactNode;
};

export default function AuthGuard({ children }: AuthGuardProps) {
    const router = useRouter();
    const pathname = usePathname();

    useEffect(() => {
        const token = getToken();

        if (!token && pathname !== "/login") {
            router.replace("/login");
            return;
        }

        if (token && pathname === "/login") {
            router.replace("/");
        }
    }, [pathname, router]);

    return <>{children}</>;
}