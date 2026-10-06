"use client";

import { FormEvent, useState } from "react";
import {
    ArrowRight,
    Eye,
    EyeOff,
    LockKeyhole,
    Mail,
    Music2,
} from "lucide-react";
import { useRouter } from "next/navigation";

import { apiPost } from "../../lib/api";
import { saveAuth, type AuthUser, type LoginResponse } from "../../lib/auth";

export default function LoginPage() {
    const router = useRouter();

    const [showPassword, setShowPassword] = useState(false);
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    const [isLoading, setIsLoading] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");

    async function handleSubmit(event: FormEvent<HTMLFormElement>) {
        event.preventDefault();

        setErrorMessage("");
        setIsLoading(true);

        try {
            const response = await apiPost<LoginResponse>("/api/Auth/login", {
                email,
                password,
            });

            const user: AuthUser = {
                id: response.userId,
                username: response.username,
                email: response.email,
                displayName: response.displayName,
                profileImageUrl: null,
                bio: null,
                role: "User",
                isActive: true,
            };

            saveAuth(response, user);

            router.push("/");
            router.refresh();
        } catch (error) {
            console.error("Roggy login failed:", error);

            setErrorMessage(
                error instanceof Error
                    ? error.message
                    : "Unable to sign in. Please check your details and try again.",
            );
        } finally {
            setIsLoading(false);
        }
    }

    return (
        <main className="relative min-h-screen overflow-hidden bg-[#05070d] text-white">
            {/* Ambient background */}
            <div className="pointer-events-none absolute inset-0 overflow-hidden">
                <div className="absolute -left-32 -top-32 h-96 w-96 rounded-full bg-cyan-500/20 blur-[120px]" />
                <div className="absolute right-[-10rem] top-[-8rem] h-[32rem] w-[32rem] rounded-full bg-purple-600/20 blur-[140px]" />
                <div className="absolute bottom-[-12rem] left-1/3 h-[30rem] w-[30rem] rounded-full bg-fuchsia-600/15 blur-[140px]" />
                <div className="absolute bottom-[-10rem] right-[-5rem] h-80 w-80 rounded-full bg-orange-500/10 blur-[120px]" />
            </div>

            <div className="relative flex min-h-screen">
                {/* Left branding panel */}
                <section className="hidden w-1/2 flex-col justify-between border-r border-white/[0.06] bg-white/[0.015] p-10 lg:flex">
                    <div>
                        <div className="flex items-center gap-3">
                            <div className="relative flex h-11 w-11 items-center justify-center">
                                <div className="absolute inset-0 rounded-xl bg-gradient-to-br from-cyan-400 via-purple-500 to-orange-500 opacity-80 blur-md" />

                                <div className="relative flex h-11 w-11 items-center justify-center rounded-xl border border-white/10 bg-[#080b14]">
                                    <span className="bg-gradient-to-br from-cyan-400 via-purple-500 to-orange-500 bg-clip-text text-3xl font-black leading-none text-transparent">
                                        R
                                    </span>
                                </div>
                            </div>

                            <div>
                                <h1 className="text-2xl font-bold tracking-tight">Roggy</h1>
                                <p className="text-[9px] font-medium uppercase tracking-[0.28em] text-zinc-500">
                                    Music Platform
                                </p>
                            </div>
                        </div>
                    </div>

                    <div className="max-w-xl">
                        <div className="mb-7 flex h-14 w-14 items-center justify-center rounded-2xl border border-white/[0.08] bg-white/[0.04] shadow-2xl">
                            <Music2 className="h-7 w-7 text-fuchsia-400" />
                        </div>

                        <h2 className="text-5xl font-black leading-[1.05] tracking-tight">
                            Your music.
                            <br />
                            <span className="bg-gradient-to-r from-cyan-400 via-purple-400 to-fuchsia-400 bg-clip-text text-transparent">
                                Your world.
                            </span>
                        </h2>

                        <p className="mt-6 max-w-lg text-sm leading-6 text-zinc-500">
                            Discover music, save your favorites, follow artists, build
                            playlists, and keep your entire listening experience in one
                            place.
                        </p>

                        <div className="mt-8 flex flex-wrap gap-2">
                            {["Discover", "Listen", "Save", "Follow", "Create"].map(
                                (item) => (
                                    <span
                                        key={item}
                                        className="rounded-full border border-white/[0.07] bg-white/[0.03] px-3 py-1.5 text-[11px] font-medium text-zinc-400"
                                    >
                                        {item}
                                    </span>
                                ),
                            )}
                        </div>
                    </div>

                    <p className="text-[11px] text-zinc-700">
                        © {new Date().getFullYear()} Roggy. All rights reserved.
                    </p>
                </section>

                {/* Login panel */}
                <section className="flex flex-1 items-center justify-center px-6 py-10 sm:px-10">
                    <div className="w-full max-w-md">
                        {/* Mobile logo */}
                        <div className="mb-10 flex items-center justify-center lg:hidden">
                            <div className="flex items-center gap-3">
                                <div className="relative flex h-10 w-10 items-center justify-center">
                                    <div className="absolute inset-0 rounded-xl bg-gradient-to-br from-cyan-400 via-purple-500 to-orange-500 opacity-80 blur-md" />

                                    <div className="relative flex h-10 w-10 items-center justify-center rounded-xl border border-white/10 bg-[#080b14]">
                                        <span className="bg-gradient-to-br from-cyan-400 via-purple-500 to-orange-500 bg-clip-text text-3xl font-black leading-none text-transparent">
                                            R
                                        </span>
                                    </div>
                                </div>

                                <div>
                                    <h1 className="text-2xl font-bold tracking-tight">Roggy</h1>
                                    <p className="text-[9px] font-medium uppercase tracking-[0.28em] text-zinc-500">
                                        Music Platform
                                    </p>
                                </div>
                            </div>
                        </div>

                        <div className="rounded-3xl border border-white/[0.08] bg-[#0a0d16]/90 p-7 shadow-2xl shadow-black/30 backdrop-blur-xl sm:p-9">
                            <div className="mb-8">
                                <p className="mb-2 text-xs font-semibold uppercase tracking-[0.2em] text-fuchsia-400">
                                    Welcome back
                                </p>

                                <h2 className="text-3xl font-bold tracking-tight text-white">
                                    Sign in to Roggy
                                </h2>

                                <p className="mt-2 text-sm text-zinc-500">
                                    Continue listening to your music.
                                </p>
                            </div>

                            <form onSubmit={handleSubmit} className="space-y-5">
                                {/* Error message */}
                                {errorMessage && (
                                    <div className="rounded-xl border border-red-500/20 bg-red-500/10 px-4 py-3 text-xs leading-5 text-red-300">
                                        {errorMessage}
                                    </div>
                                )}

                                {/* Email */}
                                <div>
                                    <label
                                        htmlFor="email"
                                        className="mb-2 block text-xs font-medium text-zinc-400"
                                    >
                                        Email
                                    </label>

                                    <div className="relative">
                                        <Mail className="pointer-events-none absolute left-4 top-1/2 h-4 w-4 -translate-y-1/2 text-zinc-600" />

                                        <input
                                            id="email"
                                            name="email"
                                            type="email"
                                            autoComplete="email"
                                            placeholder="you@example.com"
                                            value={email}
                                            onChange={(event) => setEmail(event.target.value)}
                                            required
                                            disabled={isLoading}
                                            className="h-12 w-full rounded-xl border border-white/[0.08] bg-white/[0.03] pl-11 pr-4 text-sm text-white outline-none transition placeholder:text-zinc-700 focus:border-purple-500/50 focus:bg-white/[0.045] focus:ring-2 focus:ring-purple-500/10 disabled:cursor-not-allowed disabled:opacity-50"
                                        />
                                    </div>
                                </div>

                                {/* Password */}
                                <div>
                                    <div className="mb-2 flex items-center justify-between">
                                        <label
                                            htmlFor="password"
                                            className="block text-xs font-medium text-zinc-400"
                                        >
                                            Password
                                        </label>

                                        <button
                                            type="button"
                                            disabled={isLoading}
                                            className="text-[11px] font-medium text-purple-400 transition hover:text-purple-300 disabled:cursor-not-allowed disabled:opacity-50"
                                        >
                                            Forgot password?
                                        </button>
                                    </div>

                                    <div className="relative">
                                        <LockKeyhole className="pointer-events-none absolute left-4 top-1/2 h-4 w-4 -translate-y-1/2 text-zinc-600" />

                                        <input
                                            id="password"
                                            name="password"
                                            type={showPassword ? "text" : "password"}
                                            autoComplete="current-password"
                                            placeholder="Enter your password"
                                            value={password}
                                            onChange={(event) => setPassword(event.target.value)}
                                            required
                                            disabled={isLoading}
                                            className="h-12 w-full rounded-xl border border-white/[0.08] bg-white/[0.03] pl-11 pr-12 text-sm text-white outline-none transition placeholder:text-zinc-700 focus:border-purple-500/50 focus:bg-white/[0.045] focus:ring-2 focus:ring-purple-500/10 disabled:cursor-not-allowed disabled:opacity-50"
                                        />

                                        <button
                                            type="button"
                                            aria-label={
                                                showPassword ? "Hide password" : "Show password"
                                            }
                                            onClick={() => setShowPassword((current) => !current)}
                                            disabled={isLoading}
                                            className="absolute right-3 top-1/2 -translate-y-1/2 rounded-lg p-2 text-zinc-600 transition hover:bg-white/[0.04] hover:text-zinc-300 disabled:cursor-not-allowed disabled:opacity-50"
                                        >
                                            {showPassword ? (
                                                <EyeOff className="h-4 w-4" />
                                            ) : (
                                                <Eye className="h-4 w-4" />
                                            )}
                                        </button>
                                    </div>
                                </div>

                                {/* Submit */}
                                <button
                                    type="submit"
                                    disabled={isLoading}
                                    className="group flex h-12 w-full items-center justify-center gap-2 rounded-xl bg-gradient-to-r from-purple-600 via-fuchsia-500 to-pink-500 text-sm font-semibold text-white shadow-lg shadow-purple-500/20 transition hover:scale-[1.01] hover:shadow-purple-500/30 disabled:cursor-not-allowed disabled:scale-100 disabled:opacity-60"
                                >
                                    {isLoading ? "Signing in..." : "Sign in"}

                                    {!isLoading && (
                                        <ArrowRight className="h-4 w-4 transition-transform group-hover:translate-x-1" />
                                    )}
                                </button>
                            </form>

                            <div className="my-7 flex items-center gap-4">
                                <div className="h-px flex-1 bg-white/[0.06]" />
                                <span className="text-[10px] uppercase tracking-[0.18em] text-zinc-700">
                                    Roggy
                                </span>
                                <div className="h-px flex-1 bg-white/[0.06]" />
                            </div>

                            <p className="text-center text-xs text-zinc-600">
                                New to Roggy?{" "}
                                <button
                                    type="button"
                                    className="font-medium text-purple-400 transition hover:text-purple-300"
                                >
                                    Create an account
                                </button>
                            </p>
                        </div>
                    </div>
                </section>
            </div>
        </main>
    );
}