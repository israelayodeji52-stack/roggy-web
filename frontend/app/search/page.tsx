"use client";

import { FormEvent, useState } from "react";
import {
    ArrowLeft,
    Clock3,
    Heart,
    Music2,
    Pause,
    Play,
    Search as SearchIcon,
} from "lucide-react";
import Link from "next/link";

import Sidebar from "../../Components/Sidebar";
import { apiGet } from "../../lib/api";
import { getToken } from "../../lib/auth";
import { useAudioPlayer } from "../../Components/AudioPlayerProvider";

type Song = {
    id: string;
    artistId: string;
    albumId: string | null;
    genreId: string;
    title: string;
    description: string | null;
    audioUrl: string;
    coverImageUrl: string | null;
    duration: string;
    releaseDate: string;
    contentRating: number;
    allowRemixing: boolean;
    isPublished: boolean;
    playCount: number;
    trackNumber: number;
    createdAt: string;
    updatedAt: string;
};

const gradientClasses = [
    "from-cyan-400 via-blue-500 to-purple-600",
    "from-fuchsia-500 via-purple-600 to-indigo-700",
    "from-orange-400 via-pink-500 to-purple-600",
    "from-emerald-400 via-cyan-500 to-blue-600",
    "from-rose-500 via-orange-500 to-yellow-400",
    "from-violet-500 via-fuchsia-500 to-pink-600",
];

function getGradient(index: number) {
    return gradientClasses[index % gradientClasses.length];
}

function formatDuration(duration: string) {
    const parts = duration.split(":");

    if (parts.length !== 3) {
        return duration;
    }

    const minutes = Number(parts[1]);
    const seconds = Number(parts[2]);

    if (Number.isNaN(minutes) || Number.isNaN(seconds)) {
        return duration;
    }

    return `${minutes}:${seconds.toString().padStart(2, "0")}`;
}

function SongArtwork({
    song,
    index,
}: {
    song: Song;
    index: number;
}) {
    return (
        <div
            className={`relative h-16 w-16 shrink-0 overflow-hidden rounded-xl bg-gradient-to-br ${getGradient(index)}`}
        >
            {song.coverImageUrl ? (
                <img
                    src={song.coverImageUrl}
                    alt={song.title}
                    className="h-full w-full object-cover"
                />
            ) : (
                <>
                    <div className="absolute -right-5 -top-5 h-16 w-16 rounded-full bg-white/20 blur-xl" />
                    <div className="absolute -bottom-5 -left-4 h-16 w-16 rounded-full bg-black/20 blur-xl" />

                    <div className="absolute inset-0 flex items-center justify-center">
                        <Music2 className="h-7 w-7 text-white/90" />
                    </div>
                </>
            )}

            <div className="absolute inset-0 bg-gradient-to-t from-black/30 to-transparent" />
        </div>
    );
}

function toPlayerSong(song: Song) {
    return {
        id: song.id,
        title: song.title,
        artistId: song.artistId,
        audioUrl: song.audioUrl,
        coverImageUrl: song.coverImageUrl,
        duration: song.duration,
    };
}

export default function SearchPage() {
    const {
        currentSong,
        isPlaying,
        playSong,
        togglePlay,
    } = useAudioPlayer();

    const [query, setQuery] = useState("");
    const [searchQuery, setSearchQuery] = useState("");
    const [songs, setSongs] = useState<Song[]>([]);
    const [isLoading, setIsLoading] = useState(false);
    const [apiError, setApiError] = useState("");

    async function handleSearch(event: FormEvent<HTMLFormElement>) {
        event.preventDefault();

        const trimmedQuery = query.trim();

        setApiError("");

        if (!trimmedQuery) {
            setSearchQuery("");
            setSongs([]);
            return;
        }

        setSearchQuery(trimmedQuery);
        setIsLoading(true);

        try {
            const token = getToken();

            const results = await apiGet<Song[]>(
                `/api/Songs?query=${encodeURIComponent(trimmedQuery)}`,
                token ?? undefined,
            );

            setSongs(results ?? []);
        } catch (error) {
            console.error("Roggy search failed:", error);

            setApiError(
                error instanceof Error
                    ? error.message
                    : "Unable to search Roggy.",
            );

            setSongs([]);
        } finally {
            setIsLoading(false);
        }
    }

    function handleClearSearch() {
        setQuery("");
        setSearchQuery("");
        setSongs([]);
        setApiError("");
    }

    function handlePlaySong(song: Song) {
        if (currentSong?.id === song.id) {
            togglePlay();
            return;
        }

        playSong(toPlayerSong(song));
    }

    return (
        <main className="flex min-h-screen bg-[#05070d] text-white">
            <Sidebar />

            <section className="min-w-0 flex-1 overflow-hidden">
                <div className="h-screen overflow-y-auto">
                    <header className="sticky top-0 z-20 border-b border-white/[0.06] bg-[#05070d]/90 px-6 py-5 backdrop-blur-xl">
                        <div className="mx-auto flex max-w-6xl items-center gap-4">
                            <Link
                                href="/"
                                aria-label="Back to home"
                                className="flex h-10 w-10 shrink-0 items-center justify-center rounded-xl border border-white/[0.08] bg-white/[0.03] text-zinc-400 transition hover:bg-white/[0.07] hover:text-white"
                            >
                                <ArrowLeft className="h-4 w-4" />
                            </Link>

                            <form
                                onSubmit={handleSearch}
                                className="relative min-w-0 flex-1"
                            >
                                <SearchIcon className="pointer-events-none absolute left-4 top-1/2 h-5 w-5 -translate-y-1/2 text-zinc-500" />

                                <input
                                    type="search"
                                    value={query}
                                    onChange={(event) => setQuery(event.target.value)}
                                    placeholder="Search songs, artists, albums..."
                                    className="h-12 w-full rounded-2xl border border-white/[0.08] bg-white/[0.04] pl-12 pr-28 text-sm text-white outline-none transition placeholder:text-zinc-600 focus:border-purple-500/50 focus:bg-white/[0.06] focus:ring-2 focus:ring-purple-500/10"
                                />

                                <button
                                    type="submit"
                                    disabled={isLoading}
                                    className="absolute right-2 top-1/2 -translate-y-1/2 rounded-xl bg-gradient-to-r from-fuchsia-500 to-purple-600 px-5 py-2 text-xs font-semibold text-white shadow-lg shadow-purple-500/20 transition hover:scale-[1.02] disabled:cursor-not-allowed disabled:opacity-50"
                                >
                                    Search
                                </button>
                            </form>
                        </div>
                    </header>

                    <div className="mx-auto max-w-6xl px-6 pb-32 pt-8">
                        <div className="mb-8">
                            <div className="mb-2 flex items-center gap-2">
                                <SearchIcon className="h-5 w-5 text-fuchsia-400" />

                                <h1 className="text-3xl font-bold tracking-tight">
                                    {searchQuery
                                        ? `Results for "${searchQuery}"`
                                        : "Discover music"}
                                </h1>
                            </div>

                            <p className="text-sm text-zinc-500">
                                {searchQuery
                                    ? "Find songs from your Roggy music library."
                                    : "Search through the music available on Roggy."}
                            </p>
                        </div>

                        {apiError && (
                            <div className="mb-6 rounded-2xl border border-red-500/20 bg-red-500/[0.06] px-4 py-3 text-sm text-red-300">
                                {apiError}
                            </div>
                        )}

                        {!searchQuery && !isLoading && !apiError && (
                            <div className="relative overflow-hidden rounded-3xl border border-white/[0.07] bg-gradient-to-br from-purple-950/40 via-[#0b0d18] to-cyan-950/20 px-6 py-20 text-center">
                                <div className="absolute -right-16 -top-16 h-40 w-40 rounded-full bg-fuchsia-500/20 blur-3xl" />
                                <div className="absolute -bottom-16 -left-16 h-40 w-40 rounded-full bg-cyan-500/10 blur-3xl" />

                                <div className="relative">
                                    <div className="mx-auto flex h-16 w-16 items-center justify-center rounded-2xl bg-gradient-to-br from-fuchsia-500 via-purple-600 to-cyan-500 shadow-lg shadow-purple-500/20">
                                        <SearchIcon className="h-7 w-7 text-white" />
                                    </div>

                                    <h2 className="mt-5 text-lg font-semibold text-white">
                                        What do you want to listen to?
                                    </h2>

                                    <p className="mx-auto mt-2 max-w-md text-sm leading-6 text-zinc-500">
                                        Search for a song using its title, artist, or another
                                        keyword.
                                    </p>
                                </div>
                            </div>
                        )}

                        {isLoading && (
                            <div className="grid gap-3">
                                {Array.from({ length: 5 }).map((_, index) => (
                                    <div
                                        key={index}
                                        className="flex items-center gap-4 rounded-2xl border border-white/[0.05] bg-white/[0.025] p-3"
                                    >
                                        <div className="h-16 w-16 shrink-0 animate-pulse rounded-xl bg-white/[0.07]" />

                                        <div className="min-w-0 flex-1">
                                            <div className="h-4 w-1/3 animate-pulse rounded bg-white/[0.07]" />

                                            <div className="mt-2 h-3 w-1/5 animate-pulse rounded bg-white/[0.05]" />
                                        </div>

                                        <div className="hidden h-3 w-12 animate-pulse rounded bg-white/[0.05] sm:block" />
                                    </div>
                                ))}
                            </div>
                        )}

                        {!isLoading &&
                            searchQuery &&
                            songs.length === 0 &&
                            !apiError && (
                                <div className="relative overflow-hidden rounded-3xl border border-white/[0.07] bg-gradient-to-br from-purple-950/40 via-[#0b0d18] to-cyan-950/20 px-6 py-20 text-center">
                                    <div className="absolute -right-16 -top-16 h-40 w-40 rounded-full bg-fuchsia-500/20 blur-3xl" />
                                    <div className="absolute -bottom-16 -left-16 h-40 w-40 rounded-full bg-cyan-500/10 blur-3xl" />

                                    <div className="relative">
                                        <div className="mx-auto flex h-16 w-16 items-center justify-center rounded-2xl bg-gradient-to-br from-fuchsia-500 via-purple-600 to-cyan-500 shadow-lg shadow-purple-500/20">
                                            <SearchIcon className="h-7 w-7 text-white" />
                                        </div>

                                        <h2 className="mt-5 text-lg font-semibold text-white">
                                            No songs found
                                        </h2>

                                        <p className="mx-auto mt-2 max-w-md text-sm leading-6 text-zinc-500">
                                            Try another song title, artist, or search term.
                                        </p>

                                        <button
                                            type="button"
                                            onClick={handleClearSearch}
                                            className="mt-5 rounded-full bg-white/[0.07] px-5 py-2.5 text-xs font-medium text-zinc-300 transition hover:bg-white/[0.12] hover:text-white"
                                        >
                                            Clear search
                                        </button>
                                    </div>
                                </div>
                            )}

                        {!isLoading && songs.length > 0 && (
                            <div>
                                <div className="mb-4 flex items-center justify-between">
                                    <p className="text-xs font-medium uppercase tracking-[0.18em] text-zinc-600">
                                        {songs.length}{" "}
                                        {songs.length === 1 ? "song" : "songs"}
                                    </p>

                                    <button
                                        type="button"
                                        onClick={handleClearSearch}
                                        className="text-xs font-medium text-zinc-500 transition hover:text-white"
                                    >
                                        Clear search
                                    </button>
                                </div>

                                <div className="space-y-2">
                                    {songs.map((song, index) => {
                                        const isCurrentSong = currentSong?.id === song.id;
                                        const showPause = isCurrentSong && isPlaying;

                                        return (
                                            <div
                                                key={song.id}
                                                className={`group flex items-center gap-4 rounded-2xl border p-3 transition-all duration-200 ${isCurrentSong
                                                        ? "border-fuchsia-500/30 bg-fuchsia-500/[0.06]"
                                                        : "border-white/[0.05] bg-white/[0.025] hover:border-purple-500/20 hover:bg-white/[0.05]"
                                                    }`}
                                            >
                                                <div className="relative shrink-0">
                                                    <SongArtwork song={song} index={index} />

                                                    <button
                                                        type="button"
                                                        onClick={() => handlePlaySong(song)}
                                                        aria-label={
                                                            showPause
                                                                ? `Pause ${song.title}`
                                                                : `Play ${song.title}`
                                                        }
                                                        className="absolute inset-0 flex items-center justify-center rounded-xl bg-black/60 backdrop-blur-[2px] opacity-0 transition group-hover:opacity-100"
                                                    >
                                                        <span className="flex h-9 w-9 items-center justify-center rounded-full bg-white text-black shadow-lg">
                                                            {showPause ? (
                                                                <Pause className="h-4 w-4 fill-current" />
                                                            ) : (
                                                                <Play className="ml-0.5 h-4 w-4 fill-current" />
                                                            )}
                                                        </span>
                                                    </button>
                                                </div>

                                                <div className="min-w-0 flex-1">
                                                    <h2
                                                        className={`truncate text-sm font-semibold ${isCurrentSong
                                                                ? "text-fuchsia-300"
                                                                : "text-white"
                                                            }`}
                                                    >
                                                        {song.title}
                                                    </h2>

                                                    <p className="mt-1 truncate text-xs text-zinc-500">
                                                        Roggy Artist •{" "}
                                                        {song.description ??
                                                            "No description available"}
                                                    </p>
                                                </div>

                                                <div className="hidden items-center gap-6 md:flex">
                                                    <div className="flex items-center gap-1.5 text-xs text-zinc-600">
                                                        <Clock3 className="h-3.5 w-3.5" />
                                                        {formatDuration(song.duration)}
                                                    </div>

                                                    <div className="flex items-center gap-1.5 text-xs text-zinc-600">
                                                        <Heart className="h-3.5 w-3.5" />
                                                        {song.playCount}
                                                    </div>
                                                </div>

                                                <button
                                                    type="button"
                                                    onClick={() => handlePlaySong(song)}
                                                    aria-label={
                                                        showPause
                                                            ? `Pause ${song.title}`
                                                            : `Play ${song.title}`
                                                    }
                                                    className={`flex h-9 w-9 shrink-0 items-center justify-center rounded-full transition ${isCurrentSong
                                                            ? "bg-fuchsia-500 text-white shadow-lg shadow-fuchsia-500/20"
                                                            : "bg-white/[0.05] text-zinc-500 hover:bg-fuchsia-500 hover:text-white"
                                                        }`}
                                                >
                                                    {showPause ? (
                                                        <Pause className="h-4 w-4 fill-current" />
                                                    ) : (
                                                        <Play className="h-4 w-4 fill-current" />
                                                    )}
                                                </button>
                                            </div>
                                        );
                                    })}
                                </div>
                            </div>
                        )}
                    </div>
                </div>
            </section>
        </main>
    );
}