"use client";

import {
    Clock3,
    Download,
    Heart,
    Home,
    Library,
    ListMusic,
    Plus,
    Search,
    Settings,
    Upload,
    UserRound,
} from "lucide-react";

const navigationItems = [
    {
        label: "Home",
        icon: Home,
        href: "/",
    },
    {
        label: "Search",
        icon: Search,
        href: "/search",
    },
    {
        label: "Your Library",
        icon: Library,
        href: "/library",
    },
    {
        label: "Liked Songs",
        icon: Heart,
        href: "/liked",
    },
    {
        label: "Playlists",
        icon: ListMusic,
        href: "/playlists",
    },
    {
        label: "Downloads",
        icon: Download,
        href: "/downloads",
    },
    {
        label: "History",
        icon: Clock3,
        href: "/history",
    },
];

const accountItems = [
    {
        label: "Profile",
        icon: UserRound,
        href: "/profile",
    },
    {
        label: "Settings",
        icon: Settings,
        href: "/settings",
    },
];

const playlists = [
    {
        name: "Chill Vibes",
        songs: 124,
        gradient: "from-orange-500 to-pink-600",
    },
    {
        name: "Afrobeats Hits",
        songs: 98,
        gradient: "from-cyan-500 to-blue-600",
    },
    {
        name: "Workout Mix",
        songs: 76,
        gradient: "from-green-400 to-emerald-600",
    },
    {
        name: "Love Songs",
        songs: 62,
        gradient: "from-red-500 to-rose-600",
    },
    {
        name: "New Releases",
        songs: 45,
        gradient: "from-purple-500 to-fuchsia-600",
    },
];

function RoggyLogo() {
    return (
        <div className="flex items-center gap-3">
            <div className="relative flex h-10 w-10 shrink-0 items-center justify-center">
                <div className="absolute inset-0 rounded-xl bg-gradient-to-br from-cyan-400 via-purple-500 to-orange-500 opacity-80 blur-md" />

                <div className="relative flex h-10 w-10 items-center justify-center rounded-xl border border-white/10 bg-[#080b14]">
                    <span className="bg-gradient-to-br from-cyan-400 via-purple-500 to-orange-500 bg-clip-text text-3xl font-black leading-none text-transparent">
                        R
                    </span>
                </div>
            </div>

            <div className="min-w-0">
                <h1 className="text-2xl font-bold tracking-tight text-white">
                    Roggy
                </h1>

                <p className="text-[9px] font-medium uppercase tracking-[0.28em] text-zinc-500">
                    Music Platform
                </p>
            </div>
        </div>
    );
}

export default function Sidebar() {
    return (
        <aside className="flex h-screen w-[270px] shrink-0 flex-col overflow-hidden border-r border-white/[0.06] bg-[#070a12] text-white">
            {/* Logo */}
            <div className="shrink-0 px-5 pb-5 pt-6">
                <RoggyLogo />
            </div>

            {/* Scrollable sidebar content */}
            <div className="min-h-0 flex-1 overflow-y-auto px-3 scrollbar-thin scrollbar-track-transparent scrollbar-thumb-white/10">
                {/* Navigation */}
                <nav>
                    <p className="mb-3 px-3 text-[10px] font-semibold uppercase tracking-[0.2em] text-zinc-600">
                        Discover
                    </p>

                    <div className="space-y-1">
                        {navigationItems.map((item, index) => {
                            const Icon = item.icon;
                            const isActive = index === 0;

                            return (
                                <a
                                    key={item.label}
                                    href={item.href}
                                    className={`group relative flex items-center gap-3 overflow-hidden rounded-xl px-3 py-3 text-sm font-medium transition-all duration-200 ${isActive
                                            ? "bg-gradient-to-r from-purple-600/30 via-fuchsia-500/20 to-transparent text-white shadow-[inset_0_0_20px_rgba(168,85,247,0.08)]"
                                            : "text-zinc-400 hover:bg-white/[0.04] hover:text-white"
                                        }`}
                                >
                                    {isActive && (
                                        <span className="absolute left-0 top-1/2 h-7 w-1 -translate-y-1/2 rounded-r-full bg-gradient-to-b from-cyan-400 via-purple-500 to-fuchsia-500 shadow-[0_0_12px_rgba(168,85,247,0.8)]" />
                                    )}

                                    <Icon
                                        className={`h-[19px] w-[19px] shrink-0 transition ${isActive
                                                ? "text-fuchsia-400"
                                                : "text-zinc-500 group-hover:text-cyan-400"
                                            }`}
                                    />

                                    <span>{item.label}</span>

                                    {item.label === "Liked Songs" && (
                                        <span className="ml-auto h-1.5 w-1.5 rounded-full bg-pink-500 shadow-[0_0_8px_rgba(236,72,153,0.9)]" />
                                    )}
                                </a>
                            );
                        })}
                    </div>
                </nav>

                {/* Divider */}
                <div className="mx-3 my-5 border-t border-white/[0.06]" />

                {/* Playlists */}
                <section>
                    <div className="mb-3 flex items-center justify-between px-3">
                        <p className="text-[10px] font-semibold uppercase tracking-[0.2em] text-zinc-600">
                            Playlists
                        </p>

                        <button
                            type="button"
                            aria-label="Create playlist"
                            className="rounded-lg p-1 text-zinc-500 transition hover:bg-white/[0.05] hover:text-white"
                        >
                            <Plus className="h-4 w-4" />
                        </button>
                    </div>

                    <div className="space-y-1">
                        {playlists.map((playlist) => (
                            <a
                                key={playlist.name}
                                href="/playlists"
                                className="group flex items-center gap-3 rounded-xl px-3 py-2 transition hover:bg-white/[0.04]"
                            >
                                <div
                                    className={`flex h-9 w-9 shrink-0 items-center justify-center rounded-lg bg-gradient-to-br ${playlist.gradient} shadow-lg`}
                                >
                                    <ListMusic className="h-4 w-4 text-white" />
                                </div>

                                <div className="min-w-0 flex-1">
                                    <p className="truncate text-xs font-medium text-zinc-300 transition group-hover:text-white">
                                        {playlist.name}
                                    </p>

                                    <p className="mt-0.5 text-[10px] text-zinc-600">
                                        {playlist.songs} songs
                                    </p>
                                </div>
                            </a>
                        ))}
                    </div>
                </section>

                {/* Premium */}
                <section className="relative mt-6 overflow-hidden rounded-2xl border border-white/[0.08] bg-gradient-to-br from-indigo-950 via-purple-950 to-fuchsia-950 p-4">
                    <div className="absolute -right-10 -top-10 h-28 w-28 rounded-full bg-fuchsia-500/30 blur-2xl" />

                    <div className="absolute -bottom-10 -left-8 h-24 w-24 rounded-full bg-cyan-400/20 blur-2xl" />

                    <div className="relative">
                        <div className="mb-3 flex h-9 w-9 items-center justify-center rounded-xl bg-gradient-to-br from-cyan-400 via-purple-500 to-orange-500 shadow-lg">
                            <span className="text-lg font-black text-white">R</span>
                        </div>

                        <h3 className="text-sm font-semibold text-white">
                            Roggy Premium
                        </h3>

                        <p className="mt-1 text-[11px] leading-4 text-zinc-400">
                            Ad-free. Better sound. More features.
                        </p>

                        <button
                            type="button"
                            className="mt-3 rounded-full bg-gradient-to-r from-fuchsia-500 to-purple-500 px-4 py-2 text-[11px] font-semibold text-white shadow-lg shadow-purple-500/20 transition hover:scale-105"
                        >
                            Upgrade
                        </button>
                    </div>
                </section>

                <div className="h-5" />
            </div>

            {/* Fixed bottom section */}
            <div className="shrink-0 border-t border-white/[0.06] bg-[#070a12] px-3 py-2">
                {accountItems.map((item) => {
                    const Icon = item.icon;

                    return (
                        <a
                            key={item.label}
                            href={item.href}
                            className="flex items-center gap-3 rounded-xl px-3 py-2.5 text-sm text-zinc-500 transition hover:bg-white/[0.04] hover:text-white"
                        >
                            <Icon className="h-[18px] w-[18px]" />
                            <span>{item.label}</span>
                        </a>
                    );
                })}

                <button
                    type="button"
                    className="mt-1 flex w-full items-center gap-3 rounded-xl px-3 py-2.5 text-sm text-zinc-500 transition hover:bg-white/[0.04] hover:text-white"
                >
                    <Upload className="h-[18px] w-[18px]" />
                    <span>Upload Music</span>
                </button>
            </div>
        </aside>
    );
}