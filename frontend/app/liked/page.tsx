"use client";

import { useEffect, useMemo, useState } from "react";
import {
    Clock3,
    Heart,
    MoreHorizontal,
    Pause,
    Play,
    Shuffle,
} from "lucide-react";

import Sidebar from "../../Components/Sidebar";
import {
    PlayerSong,
    useAudioPlayer,
} from "../../Components/AudioPlayerProvider";
import { apiGet } from "../../lib/api";
import { getToken } from "../../lib/auth";

type Song = {
    id: string;
    artistId: string;
    albumId: string | null;
    genreId: string | null;
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

function formatDuration(duration: string) {
    if (!duration) {
        return "0:00";
    }

    const parts = duration.split(":");

    if (parts.length !== 3) {
        return duration;
    }

    const hours = Number(parts[0]);
    const minutes = Number(parts[1]);
    const seconds = Number(parts[2]);

    if (
        Number.isNaN(hours) ||
        Number.isNaN(minutes) ||
        Number.isNaN(seconds)
    ) {
        return duration;
    }

    if (hours > 0) {
        return `${hours}:${String(minutes).padStart(2, "0")}:${String(
            seconds,
        ).padStart(2, "0")}`;
    }

    return `${minutes}:${String(seconds).padStart(2, "0")}`;
}

function formatReleaseDate(date: string) {
    if (!date) {
        return "Unknown release";
    }

    const parsedDate = new Date(date);

    if (Number.isNaN(parsedDate.getTime())) {
        return "Unknown release";
    }

    return parsedDate.toLocaleDateString("en-US", {
        month: "short",
        day: "numeric",
        year: "numeric",
    });
}

function getGradient(index: number) {
    const gradients = [
        "from-rose-500 via-pink-500 to-orange-400",
        "from-violet-500 via-purple-500 to-fuchsia-500",
        "from-blue-500 via-cyan-500 to-teal-400",
        "from-emerald-500 via-green-500 to-lime-400",
        "from-indigo-500 via-blue-500 to-purple-500",
        "from-amber-500 via-orange-500 to-red-500",
    ];

    return gradients[index % gradients.length];
}

function SongArtwork({
    song,
    index,
}: {
    song: Song;
    index: number;
}) {
    const gradient = getGradient(index);

    return (
        <div
            className={`relative h-14 w-14 shrink-0 overflow-hidden rounded-2xl bg-gradient-to-br ${gradient}`}
        >
            {song.coverImageUrl ? (
                <img
                    src={song.coverImageUrl}
                    alt={`${song.title} cover`}
                    className="h-full w-full object-cover"
                />
            ) : (
                <>
                    <div className="absolute -right-5 -top-5 h-20 w-20 rounded-full bg-white/20 blur-xl" />

                    <div className="absolute -bottom-6 -left-5 h-24 w-24 rounded-full bg-black/20 blur-xl" />

                    <div className="absolute inset-0 flex items-center justify-center">
                        <span className="text-2xl font-black text-white/90">
                            {song.title.charAt(0).toUpperCase()}
                        </span>
                    </div>
                </>
            )}
        </div>
    );
}

function toPlayerSong(song: Song): PlayerSong {
    return {
        id: song.id,
        title: song.title,
        artistId: song.artistId,
        audioUrl: song.audioUrl,
        coverImageUrl: song.coverImageUrl,
        duration: song.duration,
    };
}

function parseDurationToSeconds(duration: string) {
    if (!duration) {
        return 0;
    }

    const parts = duration.split(":").map(Number);

    if (parts.some((part) => Number.isNaN(part))) {
        return 0;
    }

    if (parts.length === 3) {
        return parts[0] * 3600 + parts[1] * 60 + parts[2];
    }

    if (parts.length === 2) {
        return parts[0] * 60 + parts[1];
    }

    return parts[0] ?? 0;
}

function formatPlayerTime(seconds: number) {
    if (!Number.isFinite(seconds) || seconds < 0) {
        return "0:00";
    }

    const totalSeconds = Math.floor(seconds);
    const minutes = Math.floor(totalSeconds / 60);
    const remainingSeconds = totalSeconds % 60;

    return `${minutes}:${String(remainingSeconds).padStart(2, "0")}`;
}

export default function LikedSongsPage() {
    const [songs, setSongs] = useState<Song[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState("");

    const {
        currentSong,
        isPlaying,
        currentTime,
        duration,
        playSong,
        togglePlay,
    } = useAudioPlayer();

    useEffect(() => {
        let isMounted = true;

        async function loadLikedSongs() {
            setIsLoading(true);
            setError("");

            const token = getToken();

            if (!token) {
                if (isMounted) {
                    setSongs([]);
                    setError("You need to be logged in to view your liked songs.");
                    setIsLoading(false);
                }

                return;
            }

            try {
                const data = await apiGet<Song[]>("/api/Songs/liked", token);

                if (isMounted) {
                    setSongs(Array.isArray(data) ? data : []);
                }
            } catch (requestError) {
                console.error("Failed to load liked songs:", requestError);

                if (isMounted) {
                    setError(
                        requestError instanceof Error
                            ? requestError.message
                            : "Unable to load your liked songs.",
                    );
                }
            } finally {
                if (isMounted) {
                    setIsLoading(false);
                }
            }
        }

        loadLikedSongs();

        return () => {
            isMounted = false;
        };
    }, []);

    const totalDuration = useMemo(() => {
        const totalSeconds = songs.reduce(
            (total, song) => total + parseDurationToSeconds(song.duration),
            0,
        );

        return formatPlayerTime(totalSeconds);
    }, [songs]);

    const handlePlay = (song: Song) => {
        if (currentSong?.id === song.id) {
            togglePlay();
            return;
        }

        playSong(toPlayerSong(song));
    };

    const handleShuffle = () => {
        if (songs.length === 0) {
            return;
        }

        const randomIndex = Math.floor(Math.random() * songs.length);
        const randomSong = songs[randomIndex];

        if (randomSong) {
            playSong(toPlayerSong(randomSong));
        }
    };

    const handlePlayAll = () => {
        const firstSong = songs[0];

        if (firstSong) {
            playSong(toPlayerSong(firstSong));
        }
    };

    const playerDuration =
        duration > 0
            ? duration
            : currentSong
                ? parseDurationToSeconds(currentSong.duration)
                : 0;

    const progress =
        playerDuration > 0
            ? Math.min((currentTime / playerDuration) * 100, 100)
            : 0;

    return (
        <div className="min-h-screen bg-[#09090b] text-white">
            <div className="flex min-h-screen">
                <Sidebar />

                <main className="min-w-0 flex-1 pb-32">
                    <div className="mx-auto max-w-[1500px] px-6 py-7 lg:px-10">
                        <header>
                            <p className="mb-2 text-sm font-medium text-rose-400">
                                YOUR MUSIC
                            </p>

                            <h1 className="text-3xl font-bold tracking-tight">
                                Liked Songs
                            </h1>

                            <p className="mt-2 text-sm text-zinc-400">
                                The songs you never want to lose.
                            </p>
                        </header>

                        <section className="mt-8 overflow-hidden rounded-3xl border border-white/10 bg-gradient-to-br from-rose-500/15 via-pink-500/[0.06] to-transparent p-6 shadow-2xl shadow-black/20">
                            <div className="flex flex-col gap-6 md:flex-row md:items-end md:justify-between">
                                <div className="flex items-center gap-5">
                                    <div className="flex h-24 w-24 shrink-0 items-center justify-center rounded-3xl bg-gradient-to-br from-rose-500 via-pink-500 to-orange-400 shadow-lg shadow-rose-500/20">
                                        <Heart
                                            size={40}
                                            fill="currentColor"
                                            className="text-white"
                                        />
                                    </div>

                                    <div>
                                        <p className="text-sm font-medium text-rose-300">
                                            LIKED MUSIC
                                        </p>

                                        <h2 className="mt-1 text-2xl font-bold">
                                            Songs You Love
                                        </h2>

                                        <p className="mt-2 text-sm text-zinc-400">
                                            {songs.length}{" "}
                                            {songs.length === 1 ? "song" : "songs"} ·{" "}
                                            {totalDuration}
                                        </p>
                                    </div>
                                </div>

                                <div className="flex gap-3">
                                    <button
                                        type="button"
                                        onClick={handleShuffle}
                                        disabled={songs.length === 0}
                                        className="inline-flex h-11 items-center gap-2 rounded-full border border-white/10 bg-white/[0.06] px-5 text-sm font-semibold text-white transition hover:bg-white/[0.1] disabled:cursor-not-allowed disabled:opacity-40"
                                    >
                                        <Shuffle size={17} />
                                        Shuffle
                                    </button>

                                    <button
                                        type="button"
                                        onClick={handlePlayAll}
                                        disabled={songs.length === 0}
                                        className="inline-flex h-11 items-center gap-2 rounded-full bg-white px-5 text-sm font-semibold text-black transition hover:bg-zinc-200 disabled:cursor-not-allowed disabled:opacity-40"
                                    >
                                        <Play size={17} fill="currentColor" />
                                        Play all
                                    </button>
                                </div>
                            </div>
                        </section>

                        <section className="mt-8">
                            <div className="mb-4 flex items-center justify-between">
                                <div className="flex items-center gap-2">
                                    <Heart
                                        size={17}
                                        fill="currentColor"
                                        className="text-rose-400"
                                    />

                                    <h2 className="text-lg font-semibold">
                                        Your Liked Songs
                                    </h2>
                                </div>

                                {!isLoading && songs.length > 0 && (
                                    <span className="text-xs text-zinc-500">
                                        {songs.length}{" "}
                                        {songs.length === 1 ? "track" : "tracks"}
                                    </span>
                                )}
                            </div>

                            {isLoading && (
                                <div className="space-y-3">
                                    {[1, 2, 3, 4].map((item) => (
                                        <div
                                            key={item}
                                            className="flex animate-pulse items-center gap-4 rounded-2xl border border-white/5 bg-white/[0.025] p-3"
                                        >
                                            <div className="h-14 w-14 shrink-0 rounded-2xl bg-white/[0.07]" />

                                            <div className="min-w-0 flex-1">
                                                <div className="h-4 w-40 rounded bg-white/[0.07]" />

                                                <div className="mt-2 h-3 w-24 rounded bg-white/[0.05]" />
                                            </div>

                                            <div className="hidden h-3 w-16 rounded bg-white/[0.05] md:block" />
                                        </div>
                                    ))}
                                </div>
                            )}

                            {!isLoading && error && (
                                <div className="rounded-2xl border border-red-500/20 bg-red-500/[0.06] p-6">
                                    <p className="text-sm font-medium text-red-300">
                                        Could not load your liked songs.
                                    </p>

                                    <p className="mt-2 text-sm text-zinc-400">
                                        {error}
                                    </p>
                                </div>
                            )}

                            {!isLoading && !error && songs.length === 0 && (
                                <div className="rounded-3xl border border-dashed border-white/10 bg-white/[0.025] px-6 py-16 text-center">
                                    <div className="mx-auto flex h-16 w-16 items-center justify-center rounded-full bg-white/[0.06]">
                                        <Heart size={28} className="text-zinc-500" />
                                    </div>

                                    <h3 className="mt-5 text-lg font-semibold">
                                        No liked songs yet
                                    </h3>

                                    <p className="mx-auto mt-2 max-w-md text-sm leading-6 text-zinc-500">
                                        When you like a song, it will appear here so you
                                        can easily find it again.
                                    </p>
                                </div>
                            )}

                            {!isLoading && !error && songs.length > 0 && (
                                <div className="overflow-hidden rounded-3xl border border-white/10 bg-white/[0.02]">
                                    <div className="hidden grid-cols-[auto_minmax(0,1fr)_180px_80px_52px] items-center gap-4 border-b border-white/10 px-5 py-3 text-xs font-medium uppercase tracking-wider text-zinc-500 md:grid">
                                        <span className="w-10 text-center">#</span>
                                        <span>Song</span>
                                        <span>Released</span>

                                        <span className="flex justify-center">
                                            <Clock3 size={15} />
                                        </span>

                                        <span />
                                    </div>

                                    <div className="divide-y divide-white/[0.06]">
                                        {songs.map((song, index) => {
                                            const isCurrentSong = currentSong?.id === song.id;

                                            return (
                                                <div
                                                    key={song.id}
                                                    className={`group grid grid-cols-[auto_minmax(0,1fr)_52px] items-center gap-4 px-4 py-3 transition md:grid-cols-[auto_minmax(0,1fr)_180px_80px_52px] md:px-5 ${isCurrentSong
                                                            ? "bg-white/[0.07]"
                                                            : "hover:bg-white/[0.045]"
                                                        }`}
                                                >
                                                    <button
                                                        type="button"
                                                        onClick={() => handlePlay(song)}
                                                        className="relative flex h-10 w-10 items-center justify-center text-sm font-medium text-zinc-500"
                                                        aria-label={
                                                            isCurrentSong && isPlaying
                                                                ? `Pause ${song.title}`
                                                                : `Play ${song.title}`
                                                        }
                                                    >
                                                        <span className="transition group-hover:opacity-0">
                                                            {index + 1}
                                                        </span>

                                                        <span className="absolute inset-0 flex items-center justify-center opacity-0 transition group-hover:opacity-100">
                                                            {isCurrentSong && isPlaying ? (
                                                                <Pause size={17} fill="currentColor" />
                                                            ) : (
                                                                <Play size={17} fill="currentColor" />
                                                            )}
                                                        </span>
                                                    </button>

                                                    <div className="flex min-w-0 items-center gap-3">
                                                        <SongArtwork
                                                            song={song}
                                                            index={index}
                                                        />

                                                        <div className="min-w-0">
                                                            <p
                                                                className={`truncate text-sm font-semibold ${isCurrentSong
                                                                        ? "text-rose-300"
                                                                        : "text-white"
                                                                    }`}
                                                            >
                                                                {song.title}
                                                            </p>

                                                            <p className="mt-1 truncate text-xs text-zinc-500">
                                                                Artist · {song.playCount} plays
                                                            </p>
                                                        </div>
                                                    </div>

                                                    <p className="hidden truncate text-sm text-zinc-500 md:block">
                                                        {formatReleaseDate(song.releaseDate)}
                                                    </p>

                                                    <p className="hidden text-center text-sm text-zinc-500 md:block">
                                                        {formatDuration(song.duration)}
                                                    </p>

                                                    <button
                                                        type="button"
                                                        className="flex h-9 w-9 items-center justify-center rounded-full text-zinc-500 transition hover:bg-white/[0.08] hover:text-white"
                                                        aria-label={`More options for ${song.title}`}
                                                    >
                                                        <MoreHorizontal size={18} />
                                                    </button>
                                                </div>
                                            );
                                        })}
                                    </div>
                                </div>
                            )}
                        </section>
                    </div>
                </main>
            </div>

            {currentSong && (
                <div className="fixed bottom-0 left-0 right-0 z-50 border-t border-white/10 bg-[#0d0d0f]/95 px-4 py-3 backdrop-blur-2xl md:left-[250px]">
                    <div className="mx-auto max-w-[1500px]">
                        <div className="flex items-center gap-3 md:gap-5">
                            <div className="hidden items-center gap-3 sm:flex">
                                {currentSong.coverImageUrl ? (
                                    <img
                                        src={currentSong.coverImageUrl}
                                        alt={`${currentSong.title} cover`}
                                        className="h-11 w-11 rounded-xl object-cover"
                                    />
                                ) : (
                                    <div className="flex h-11 w-11 items-center justify-center rounded-xl bg-gradient-to-br from-rose-500 to-orange-400 text-sm font-bold"
                                    >
                                        {currentSong.title.charAt(0).toUpperCase()}
                                    </div>
                                )}

                                <div className="max-w-[180px]">
                                    <p className="truncate text-sm font-semibold text-white">
                                        {currentSong.title}
                                    </p>

                                    <p className="truncate text-xs text-zinc-500">
                                        Roggy
                                    </p>
                                </div>
                            </div>

                            <button
                                type="button"
                                onClick={togglePlay}
                                className="flex h-10 w-10 shrink-0 items-center justify-center rounded-full bg-white text-black transition hover:bg-zinc-200"
                                aria-label={isPlaying ? "Pause" : "Play"}
                            >
                                {isPlaying ? (
                                    <Pause size={17} fill="currentColor" />
                                ) : (
                                    <Play size={17} fill="currentColor" />
                                )}
                            </button>

                            <div className="min-w-0 flex-1">
                                <div className="flex items-center gap-2">
                                    <span className="w-9 text-right text-[11px] text-zinc-500">
                                        {formatPlayerTime(currentTime)}
                                    </span>

                                    <div className="relative h-1 flex-1 overflow-hidden rounded-full bg-white/10">
                                        <div
                                            className="absolute left-0 top-0 h-full rounded-full bg-white transition-[width]"
                                            style={{ width: `${progress}%` }}
                                        />
                                    </div>

                                    <span className="w-9 text-[11px] text-zinc-500">
                                        {formatPlayerTime(playerDuration)}
                                    </span>
                                </div>
                            </div>

                            <div className="hidden items-center gap-3 lg:flex">
                                <span className="text-xs text-zinc-500">
                                    {isPlaying ? "Playing" : "Paused"}
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}