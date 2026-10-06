"use client";

import { useEffect, useMemo, useState } from "react";
import {
    Check,
    Clock3,
    ListMusic,
    MoreHorizontal,
    Play,
    Plus,
    Shuffle,
    X,
} from "lucide-react";

import Sidebar from "../../Components/Sidebar";
import { PlayerSong, useAudioPlayer } from "../../Components/AudioPlayerProvider";
import { apiGet, apiPost } from "../../lib/api";
import { getToken } from "../../lib/auth";

type Playlist = {
    id: string;
    name: string;
    description: string | null;
    coverImageUrl: string | null;
    isPublic: boolean;
    createdAt: string;
    updatedAt: string;
    songCount?: number;
    songs?: Song[];
};

type Song = {
    id: string;
    title: string;
    artistId: string;
    albumId: string | null;
    genreId: string | null;
    duration: string;
    audioUrl: string;
    coverImageUrl: string | null;
    releaseDate: string | null;
    isPublished: boolean;
    allowRemixing: boolean;
    playCount: number;
};

type CreatePlaylistResponse = Playlist;

function parseDurationToSeconds(duration: string) {
    if (!duration) {
        return 0;
    }

    if (duration.includes(":")) {
        const parts = duration.split(":").map(Number);

        if (parts.length === 3) {
            return (
                (parts[0] || 0) * 3600 +
                (parts[1] || 0) * 60 +
                (parts[2] || 0)
            );
        }

        if (parts.length === 2) {
            return (parts[0] || 0) * 60 + (parts[1] || 0);
        }
    }

    const seconds = Number(duration);

    return Number.isFinite(seconds) ? seconds : 0;
}

function formatPlayerTime(seconds: number) {
    if (!Number.isFinite(seconds) || seconds < 0) {
        return "0:00";
    }

    const minutes = Math.floor(seconds / 60);
    const remainingSeconds = Math.floor(seconds % 60);

    return `${minutes}:${String(remainingSeconds).padStart(2, "0")}`;
}

function getGradient(index: number) {
    const gradients = [
        "from-fuchsia-500 via-purple-500 to-indigo-600",
        "from-cyan-400 via-blue-500 to-indigo-700",
        "from-orange-400 via-pink-500 to-purple-700",
        "from-emerald-400 via-teal-500 to-cyan-700",
        "from-yellow-400 via-orange-500 to-red-600",
    ];

    return gradients[index % gradients.length];
}

function PlaylistArtwork({
    playlist,
    index,
    large = false,
}: {
    playlist: Playlist;
    index: number;
    large?: boolean;
}) {
    if (playlist.coverImageUrl) {
        return (
            <img
                src={playlist.coverImageUrl}
                alt={playlist.name}
                className={`aspect-square w-full object-cover ${large ? "rounded-3xl" : "rounded-2xl"
                    }`}
            />
        );
    }

    return (
        <div
            className={`aspect-square w-full bg-gradient-to-br ${getGradient(
                index,
            )} ${large ? "rounded-3xl" : "rounded-2xl"
                } flex items-center justify-center shadow-2xl`}
        >
            <ListMusic
                size={large ? 58 : 34}
                strokeWidth={1.5}
                className="text-white/90"
            />
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

export default function PlaylistsPage() {
    const [playlists, setPlaylists] = useState<Playlist[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
    const [playlistName, setPlaylistName] = useState("");
    const [playlistDescription, setPlaylistDescription] = useState("");
    const [isPublic, setIsPublic] = useState(false);
    const [isCreating, setIsCreating] = useState(false);
    const [createError, setCreateError] = useState<string | null>(null);

    const {
        currentSong,
        isPlaying,
        currentTime,
        duration,
        playSong,
        togglePlay,
        seek,
    } = useAudioPlayer();

    async function loadPlaylists() {
        try {
            setIsLoading(true);
            setError(null);

            const token = getToken();

            if (!token) {
                throw new Error(
                    "You need to be logged in to view your playlists.",
                );
            }

            const data = await apiGet<Playlist[]>("/api/Playlist", token);

            setPlaylists(Array.isArray(data) ? data : []);
        } catch (requestError) {
            setError(
                requestError instanceof Error
                    ? requestError.message
                    : "Unable to load your playlists.",
            );
        } finally {
            setIsLoading(false);
        }
    }

    useEffect(() => {
        let cancelled = false;

        async function loadInitialPlaylists() {
            try {
                setIsLoading(true);
                setError(null);

                const token = getToken();

                if (!token) {
                    throw new Error(
                        "You need to be logged in to view your playlists.",
                    );
                }

                const data = await apiGet<Playlist[]>("/api/Playlist", token);

                if (!cancelled) {
                    setPlaylists(Array.isArray(data) ? data : []);
                }
            } catch (requestError) {
                if (cancelled) {
                    return;
                }

                setError(
                    requestError instanceof Error
                        ? requestError.message
                        : "Unable to load your playlists.",
                );
            } finally {
                if (!cancelled) {
                    setIsLoading(false);
                }
            }
        }

        loadInitialPlaylists();

        return () => {
            cancelled = true;
        };
    }, []);

    const totalSongs = useMemo(() => {
        return playlists.reduce(
            (total, playlist) =>
                total + (playlist.songCount ?? playlist.songs?.length ?? 0),
            0,
        );
    }, [playlists]);

    const visiblePlaylists = useMemo(() => {
        return playlists.slice(0, 6);
    }, [playlists]);

    function openCreateModal() {
        setPlaylistName("");
        setPlaylistDescription("");
        setIsPublic(false);
        setCreateError(null);
        setIsCreateModalOpen(true);
    }

    function closeCreateModal() {
        if (isCreating) {
            return;
        }

        setIsCreateModalOpen(false);
        setCreateError(null);
    }

    async function handleCreatePlaylist() {
        const trimmedName = playlistName.trim();
        const trimmedDescription = playlistDescription.trim();

        if (!trimmedName) {
            setCreateError("Please enter a playlist name.");
            return;
        }

        try {
            setIsCreating(true);
            setCreateError(null);

            const token = getToken();

            if (!token) {
                throw new Error(
                    "Your login session has expired. Please log in again.",
                );
            }

            const createdPlaylist = await apiPost<CreatePlaylistResponse>(
                "/api/Playlist",
                {
                    name: trimmedName,
                    description: trimmedDescription || null,
                    coverImageUrl: null,
                    isPublic,
                },
                token,
            );

            setPlaylists((currentPlaylists) => [
                createdPlaylist,
                ...currentPlaylists,
            ]);

            setIsCreateModalOpen(false);
            setPlaylistName("");
            setPlaylistDescription("");
            setIsPublic(false);
        } catch (requestError) {
            setCreateError(
                requestError instanceof Error
                    ? requestError.message
                    : "Unable to create the playlist.",
            );
        } finally {
            setIsCreating(false);
        }
    }

    function handlePlaylistPlay(playlist: Playlist) {
        const songs = playlist.songs ?? [];

        if (songs.length === 0) {
            return;
        }

        playSong(toPlayerSong(songs[0]));
    }

    function handlePlayAll() {
        const firstPlaylist = playlists.find(
            (playlist) => playlist.songs && playlist.songs.length > 0,
        );

        if (!firstPlaylist?.songs?.length) {
            return;
        }

        playSong(toPlayerSong(firstPlaylist.songs[0]));
    }

    function handleShuffle() {
        const songs = playlists.flatMap((playlist) => playlist.songs ?? []);

        if (songs.length === 0) {
            return;
        }

        const randomSong = songs[Math.floor(Math.random() * songs.length)];

        playSong(toPlayerSong(randomSong));
    }

    const currentDuration =
        duration > 0
            ? duration
            : currentSong
                ? parseDurationToSeconds(currentSong.duration)
                : 0;

    return (
        <main className="min-h-screen bg-[#05070d] text-white">
            <Sidebar />

            <section className="min-h-screen pb-32 pl-0 lg:pl-[260px]">
                <div className="mx-auto max-w-[1500px] px-5 py-6 sm:px-8 lg:px-10">
                    <header className="mb-10 flex flex-col gap-6 border-b border-white/10 pb-8 md:flex-row md:items-end md:justify-between">
                        <div>
                            <div className="mb-3 flex items-center gap-2 text-sm text-purple-300">
                                <ListMusic size={17} />
                                <span>Your collection</span>
                            </div>

                            <h1 className="text-4xl font-bold tracking-tight sm:text-5xl">
                                Playlists
                            </h1>

                            <p className="mt-3 max-w-2xl text-sm leading-6 text-white/50 sm:text-base">
                                Organize the music you love into collections that fit every
                                mood, moment, and memory.
                            </p>
                        </div>

                        <button
                            type="button"
                            onClick={openCreateModal}
                            className="inline-flex h-12 items-center justify-center gap-2 rounded-2xl bg-white px-5 text-sm font-semibold text-black transition hover:bg-white/90"
                        >
                            <Plus size={18} />
                            Create playlist
                        </button>
                    </header>

                    {isLoading ? (
                        <div className="grid grid-cols-1 gap-5 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
                            {Array.from({ length: 4 }).map((_, index) => (
                                <div
                                    key={index}
                                    className="animate-pulse rounded-3xl border border-white/10 bg-white/[0.03] p-4"
                                >
                                    <div className="aspect-square rounded-2xl bg-white/10" />
                                    <div className="mt-4 h-5 w-2/3 rounded bg-white/10" />
                                    <div className="mt-2 h-4 w-1/2 rounded bg-white/10" />
                                </div>
                            ))}
                        </div>
                    ) : error ? (
                        <div className="rounded-3xl border border-red-400/20 bg-red-500/10 p-8 text-center">
                            <h2 className="text-lg font-semibold">
                                Could not load your playlists
                            </h2>

                            <p className="mt-2 text-sm text-white/50">{error}</p>
                        </div>
                    ) : playlists.length === 0 ? (
                        <div className="relative overflow-hidden rounded-[32px] border border-white/10 bg-gradient-to-br from-purple-500/10 via-white/[0.03] to-indigo-500/10 p-8 sm:p-12">
                            <div className="absolute -right-24 -top-24 h-64 w-64 rounded-full bg-purple-500/10 blur-3xl" />

                            <div className="relative mx-auto flex max-w-2xl flex-col items-center text-center">
                                <div className="mb-6 flex h-20 w-20 items-center justify-center rounded-3xl bg-gradient-to-br from-purple-500 to-indigo-600 shadow-2xl shadow-purple-900/30">
                                    <ListMusic size={36} />
                                </div>

                                <h2 className="text-2xl font-bold sm:text-3xl">
                                    Your playlists are waiting
                                </h2>

                                <p className="mt-3 text-sm leading-6 text-white/50 sm:text-base">
                                    You haven&apos;t created a playlist yet. Build your first
                                    collection and keep your favorite songs together.
                                </p>

                                <button
                                    type="button"
                                    onClick={openCreateModal}
                                    className="mt-7 inline-flex h-12 items-center justify-center gap-2 rounded-2xl bg-white px-6 text-sm font-semibold text-black transition hover:bg-white/90"
                                >
                                    <Plus size={18} />
                                    Create your first playlist
                                </button>
                            </div>
                        </div>
                    ) : (
                        <>
                            <div className="mb-8 flex flex-wrap items-center gap-3">
                                <button
                                    type="button"
                                    onClick={handlePlayAll}
                                    className="inline-flex h-11 items-center gap-2 rounded-2xl bg-white px-5 text-sm font-semibold text-black transition hover:bg-white/90"
                                >
                                    <Play size={17} fill="currentColor" />
                                    Play all
                                </button>

                                <button
                                    type="button"
                                    onClick={handleShuffle}
                                    className="inline-flex h-11 items-center gap-2 rounded-2xl border border-white/10 bg-white/[0.04] px-5 text-sm font-semibold text-white transition hover:bg-white/[0.08]"
                                >
                                    <Shuffle size={17} />
                                    Shuffle
                                </button>

                                <div className="ml-auto flex items-center gap-4 text-sm text-white/40">
                                    <span>{playlists.length} playlists</span>
                                    <span>{totalSongs} songs</span>
                                </div>
                            </div>

                            <div className="grid grid-cols-1 gap-5 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
                                {visiblePlaylists.map((playlist, index) => {
                                    const songs = playlist.songs ?? [];
                                    const isCurrentPlaylistPlaying =
                                        songs.length > 0 &&
                                        currentSong?.id === songs[0]?.id &&
                                        isPlaying;

                                    return (
                                        <article
                                            key={playlist.id}
                                            className="group overflow-hidden rounded-3xl border border-white/10 bg-white/[0.035] p-4 transition duration-300 hover:-translate-y-1 hover:border-white/20 hover:bg-white/[0.055]"
                                        >
                                            <div className="relative">
                                                <PlaylistArtwork
                                                    playlist={playlist}
                                                    index={index}
                                                    large
                                                />

                                                <button
                                                    type="button"
                                                    onClick={() => handlePlaylistPlay(playlist)}
                                                    disabled={songs.length === 0}
                                                    className="absolute bottom-4 right-4 flex h-12 w-12 items-center justify-center rounded-full bg-white text-black shadow-xl transition hover:scale-105 disabled:cursor-not-allowed disabled:opacity-40"
                                                    aria-label={`Play ${playlist.name}`}
                                                >
                                                    {isCurrentPlaylistPlaying ? (
                                                        <span className="text-xs font-bold">II</span>
                                                    ) : (
                                                        <Play size={19} fill="currentColor" />
                                                    )}
                                                </button>
                                            </div>

                                            <div className="mt-4 flex items-start justify-between gap-3">
                                                <div className="min-w-0">
                                                    <h2 className="truncate text-base font-semibold">
                                                        {playlist.name}
                                                    </h2>

                                                    <p className="mt-1 truncate text-sm text-white/40">
                                                        {playlist.songCount ??
                                                            playlist.songs?.length ??
                                                            0}{" "}
                                                        songs
                                                        {playlist.isPublic
                                                            ? " • Public"
                                                            : " • Private"}
                                                    </p>
                                                </div>

                                                <button
                                                    type="button"
                                                    className="rounded-full p-2 text-white/30 transition hover:bg-white/10 hover:text-white"
                                                    aria-label={`More options for ${playlist.name}`}
                                                >
                                                    <MoreHorizontal size={18} />
                                                </button>
                                            </div>
                                        </article>
                                    );
                                })}
                            </div>
                        </>
                    )}
                </div>
            </section>

            {isCreateModalOpen && (
                <div
                    className="fixed inset-0 z-[100] flex items-center justify-center bg-black/70 px-5 backdrop-blur-md"
                    onMouseDown={(event) => {
                        if (event.target === event.currentTarget) {
                            closeCreateModal();
                        }
                    }}
                >
                    <div className="w-full max-w-lg overflow-hidden rounded-[28px] border border-white/10 bg-[#0d1018] shadow-2xl shadow-black/50">
                        <div className="flex items-center justify-between border-b border-white/10 px-6 py-5">
                            <div>
                                <h2 className="text-xl font-semibold">
                                    Create playlist
                                </h2>

                                <p className="mt-1 text-sm text-white/40">
                                    Give your new collection a name.
                                </p>
                            </div>

                            <button
                                type="button"
                                onClick={closeCreateModal}
                                disabled={isCreating}
                                className="rounded-full p-2 text-white/40 transition hover:bg-white/10 hover:text-white disabled:opacity-40"
                                aria-label="Close create playlist dialog"
                            >
                                <X size={20} />
                            </button>
                        </div>

                        <div className="space-y-5 px-6 py-6">
                            <div>
                                <label
                                    htmlFor="playlist-name"
                                    className="mb-2 block text-sm font-medium text-white/80"
                                >
                                    Playlist name
                                </label>

                                <input
                                    id="playlist-name"
                                    type="text"
                                    value={playlistName}
                                    onChange={(event) =>
                                        setPlaylistName(event.target.value)
                                    }
                                    placeholder="e.g. Late Night Vibes"
                                    maxLength={100}
                                    autoFocus
                                    className="h-12 w-full rounded-2xl border border-white/10 bg-white/[0.04] px-4 text-sm text-white outline-none placeholder:text-white/25 focus:border-purple-400/50 focus:bg-white/[0.06]"
                                />
                            </div>

                            <div>
                                <label
                                    htmlFor="playlist-description"
                                    className="mb-2 block text-sm font-medium text-white/80"
                                >
                                    Description
                                    <span className="ml-2 text-white/25">
                                        Optional
                                    </span>
                                </label>

                                <textarea
                                    id="playlist-description"
                                    value={playlistDescription}
                                    onChange={(event) =>
                                        setPlaylistDescription(event.target.value)
                                    }
                                    placeholder="What's this playlist about?"
                                    maxLength={500}
                                    rows={4}
                                    className="w-full resize-none rounded-2xl border border-white/10 bg-white/[0.04] px-4 py-3 text-sm text-white outline-none placeholder:text-white/25 focus:border-purple-400/50 focus:bg-white/[0.06]"
                                />
                            </div>

                            <button
                                type="button"
                                onClick={() => setIsPublic((value) => !value)}
                                className="flex w-full items-center justify-between rounded-2xl border border-white/10 bg-white/[0.03] px-4 py-4 text-left transition hover:bg-white/[0.05]"
                            >
                                <div>
                                    <p className="text-sm font-medium">
                                        Public playlist
                                    </p>

                                    <p className="mt-1 text-xs text-white/35">
                                        {isPublic
                                            ? "Other Roggy users can discover it."
                                            : "Only you can see this playlist."}
                                    </p>
                                </div>

                                <div
                                    className={`flex h-6 w-6 items-center justify-center rounded-full border transition ${isPublic
                                            ? "border-purple-400 bg-purple-500"
                                            : "border-white/20 bg-transparent"
                                        }`}
                                >
                                    {isPublic && (
                                        <Check size={14} strokeWidth={3} />
                                    )}
                                </div>
                            </button>

                            {createError && (
                                <div className="rounded-2xl border border-red-400/20 bg-red-500/10 px-4 py-3 text-sm text-red-200">
                                    {createError}
                                </div>
                            )}
                        </div>

                        <div className="flex gap-3 border-t border-white/10 px-6 py-5">
                            <button
                                type="button"
                                onClick={closeCreateModal}
                                disabled={isCreating}
                                className="h-12 flex-1 rounded-2xl border border-white/10 bg-white/[0.03] text-sm font-semibold text-white transition hover:bg-white/[0.07] disabled:opacity-40"
                            >
                                Cancel
                            </button>

                            <button
                                type="button"
                                onClick={handleCreatePlaylist}
                                disabled={isCreating || !playlistName.trim()}
                                className="h-12 flex-1 rounded-2xl bg-white text-sm font-semibold text-black transition hover:bg-white/90 disabled:cursor-not-allowed disabled:opacity-40"
                            >
                                {isCreating ? "Creating..." : "Create playlist"}
                            </button>
                        </div>
                    </div>
                </div>
            )}

            {currentSong && (
                <div className="fixed bottom-0 left-0 right-0 z-50 border-t border-white/10 bg-[#090b12]/95 px-4 py-3 backdrop-blur-2xl lg:left-[260px]">
                    <div className="mx-auto flex max-w-[1500px] items-center gap-4">
                        <div className="hidden min-w-0 flex-1 items-center gap-3 sm:flex">
                            {currentSong.coverImageUrl ? (
                                <img
                                    src={currentSong.coverImageUrl}
                                    alt={currentSong.title}
                                    className="h-12 w-12 rounded-xl object-cover"
                                />
                            ) : (
                                <div className="flex h-12 w-12 items-center justify-center rounded-xl bg-gradient-to-br from-purple-500 to-indigo-600">
                                    <ListMusic size={19} />
                                </div>
                            )}

                            <div className="min-w-0">
                                <p className="truncate text-sm font-semibold">
                                    {currentSong.title}
                                </p>
                                <p className="truncate text-xs text-white/40">
                                    Roggy player
                                </p>
                            </div>
                        </div>

                        <button
                            type="button"
                            onClick={togglePlay}
                            className="flex h-11 w-11 shrink-0 items-center justify-center rounded-full bg-white text-black transition hover:scale-105"
                            aria-label={isPlaying ? "Pause" : "Play"}
                        >
                            {isPlaying ? (
                                <span className="text-xs font-bold">II</span>
                            ) : (
                                <Play size={17} fill="currentColor" />
                            )}
                        </button>

                        <div className="hidden flex-1 items-center gap-3 md:flex">
                            <span className="w-10 text-right text-xs text-white/40">
                                {formatPlayerTime(currentTime)}
                            </span>

                            <input
                                type="range"
                                min={0}
                                max={currentDuration || 1}
                                value={Math.min(
                                    currentTime,
                                    currentDuration || 1,
                                )}
                                onChange={(event) =>
                                    seek(Number(event.target.value))
                                }
                                className="h-1 w-full cursor-pointer accent-white"
                            />

                            <span className="w-10 text-xs text-white/40">
                                {formatPlayerTime(currentDuration)}
                            </span>
                        </div>

                        <Clock3
                            className="hidden text-white/20 lg:block"
                            size={18}
                        />
                    </div>
                </div>
            )}
        </main>
    );
}