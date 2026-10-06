"use client";

import { useEffect, useMemo, useState } from "react";
import {
  Bell,
  ChevronDown,
  Clock3,
  Flame,
  Heart,
  MoreHorizontal,
  Pause,
  Play,
  Search,
  Shuffle,
  SkipBack,
  SkipForward,
  Volume2,
} from "lucide-react";
import Link from "next/link";
import { useRouter } from "next/navigation";

import Sidebar from "../Components/Sidebar";
import { useAudioPlayer } from "../Components/AudioPlayerProvider";
import { apiGet } from "../lib/api";
import { getToken } from "../lib/auth";

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

const fallbackSongs: Song[] = [
  {
    id: "demo-1",
    artistId: "demo-artist",
    albumId: null,
    genreId: "demo-genre",
    title: "Playlist Test Song 2",
    description: "Second song for playlist testing.",
    audioUrl: "",
    coverImageUrl: null,
    duration: "00:04:00",
    releaseDate: "2026-09-08T00:00:00Z",
    contentRating: 1,
    allowRemixing: false,
    isPublished: true,
    playCount: 128,
    trackNumber: 1,
    createdAt: "2026-09-08T13:07:14.188658Z",
    updatedAt: "2026-09-08T13:07:14.188658Z",
  },
  {
    id: "demo-2",
    artistId: "demo-artist",
    albumId: null,
    genreId: "demo-genre",
    title: "Test Search Song",
    description: "Song created to test Roggy search.",
    audioUrl: "",
    coverImageUrl: null,
    duration: "00:03:30",
    releaseDate: "2026-09-08T00:00:00Z",
    contentRating: 1,
    allowRemixing: true,
    isPublished: true,
    playCount: 96,
    trackNumber: 1,
    createdAt: "2026-09-08T11:08:26.263884Z",
    updatedAt: "2026-09-08T11:08:26.263884Z",
  },
];

const gradientClasses = [
  "from-cyan-400 via-blue-500 to-purple-600",
  "from-fuchsia-500 via-purple-600 to-indigo-700",
  "from-orange-400 via-pink-500 to-purple-600",
  "from-emerald-400 via-cyan-500 to-blue-600",
  "from-rose-500 via-orange-500 to-yellow-400",
  "from-violet-500 via-fuchsia-500 to-pink-600",
];

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

function formatReleaseDate(date: string) {
  const parsedDate = new Date(date);

  if (Number.isNaN(parsedDate.getTime())) {
    return "Recently released";
  }

  return parsedDate.toLocaleDateString("en-US", {
    month: "short",
    day: "numeric",
    year: "numeric",
  });
}

function getGradient(index: number) {
  return gradientClasses[index % gradientClasses.length];
}

function SongArtwork({
  song,
  index,
  size = "large",
}: {
  song: Song;
  index: number;
  size?: "small" | "medium" | "large";
}) {
  const sizeClasses = {
    small: "h-12 w-12 rounded-xl",
    medium: "h-16 w-16 rounded-2xl",
    large: "aspect-square w-full rounded-2xl",
  };

  return (
    <div
      className={`relative overflow-hidden bg-gradient-to-br ${getGradient(index)} ${sizeClasses[size]}`}
    >
      {song.coverImageUrl ? (
        <img
          src={song.coverImageUrl}
          alt={song.title}
          className="h-full w-full object-cover"
        />
      ) : (
        <>
          <div className="absolute -right-6 -top-6 h-24 w-24 rounded-full bg-white/20 blur-2xl" />
          <div className="absolute -bottom-8 -left-5 h-28 w-28 rounded-full bg-black/20 blur-2xl" />

          <div className="absolute inset-0 flex items-center justify-center">
            <span className="text-5xl font-black text-white/90 drop-shadow-lg">
              {song.title.charAt(0).toUpperCase()}
            </span>
          </div>
        </>
      )}

      <div className="absolute inset-0 bg-gradient-to-t from-black/35 to-transparent" />
    </div>
  );
}

function SectionHeader({
  title,
  subtitle,
  icon,
  href = "/search",
}: {
  title: string;
  subtitle?: string;
  icon?: React.ReactNode;
  href?: string;
}) {
  return (
    <div className="mb-5 flex items-end justify-between">
      <div>
        <div className="flex items-center gap-2">
          {icon}

          <h2 className="text-xl font-bold tracking-tight text-white">
            {title}
          </h2>
        </div>

        {subtitle && (
          <p className="mt-1 text-xs text-zinc-500">{subtitle}</p>
        )}
      </div>

      <Link
        href={href}
        className="text-xs font-medium text-zinc-500 transition hover:text-white"
      >
        See all
      </Link>
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

export default function HomePage() {
  const router = useRouter();

  const {
    currentSong,
    isPlaying,
    currentTime,
    duration,
    playSong,
    togglePlay,
    seek,
    next,
    previous,
  } = useAudioPlayer();

  const [recentSongs, setRecentSongs] = useState<Song[]>([]);
  const [trendingSongs, setTrendingSongs] = useState<Song[]>([]);
  const [recommendedSongs, setRecommendedSongs] = useState<Song[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [apiError, setApiError] = useState<string | null>(null);

  useEffect(() => {
    let cancelled = false;

    async function loadHomeData() {
      setIsLoading(true);
      setApiError(null);

      try {
        const token = getToken();

        const [recent, trending, recommended] = await Promise.all([
          apiGet<Song[]>(
            "/api/Songs/recently-released",
            token ?? undefined,
          ),
          apiGet<Song[]>(
            "/api/Songs/trending",
            token ?? undefined,
          ),
          apiGet<Song[]>(
            "/api/Songs/recommended",
            token ?? undefined,
          ),
        ]);

        if (cancelled) {
          return;
        }

        setRecentSongs(recent ?? []);
        setTrendingSongs(trending ?? []);
        setRecommendedSongs(recommended ?? []);
      } catch (error) {
        if (cancelled) {
          return;
        }

        console.error("Failed to load Roggy home data:", error);

        setApiError(
          error instanceof Error
            ? error.message
            : "Unable to connect to the Roggy API.",
        );

        setRecentSongs(fallbackSongs);
        setTrendingSongs(fallbackSongs);
        setRecommendedSongs(fallbackSongs);
      } finally {
        if (!cancelled) {
          setIsLoading(false);
        }
      }
    }

    loadHomeData();

    return () => {
      cancelled = true;
    };
  }, []);

  const featuredSong = useMemo(() => {
    return (
      trendingSongs[0] ??
      recentSongs[0] ??
      recommendedSongs[0] ??
      fallbackSongs[0]
    );
  }, [recentSongs, trendingSongs, recommendedSongs]);

  const handlePlay = (song: Song) => {
    if (currentSong?.id === song.id) {
      togglePlay();
      return;
    }

    playSong(toPlayerSong(song));
  };

  const handleShuffle = () => {
    const songs = [
      ...trendingSongs,
      ...recentSongs,
      ...recommendedSongs,
    ].filter(
      (song, index, array) =>
        array.findIndex((item) => item.id === song.id) === index,
    );

    const availableSongs = songs.length > 0 ? songs : fallbackSongs;

    const randomSong =
      availableSongs[Math.floor(Math.random() * availableSongs.length)];

    if (randomSong) {
      playSong(toPlayerSong(randomSong));
    }
  };

  const handleTogglePlay = () => {
    if (!currentSong) {
      return;
    }

    togglePlay();
  };

  const handleSearchClick = () => {
    router.push("/search");
  };

  const displayedRecentSongs =
    recentSongs.length > 0 ? recentSongs : fallbackSongs;

  const displayedTrendingSongs =
    trendingSongs.length > 0 ? trendingSongs : fallbackSongs;

  const displayedRecommendedSongs =
    recommendedSongs.length > 0 ? recommendedSongs : fallbackSongs;

  const playerDuration =
    duration > 0
      ? duration
      : currentSong
        ? parseDurationToSeconds(currentSong.duration)
        : 0;

  const progressPercentage =
    playerDuration > 0
      ? Math.min((currentTime / playerDuration) * 100, 100)
      : 0;

  return (
    <main className="flex h-screen overflow-hidden bg-[#05070d] text-white">
      <Sidebar />

      <div className="min-w-0 flex-1 overflow-y-auto">
        <div className="min-h-full bg-[radial-gradient(circle_at_top_right,rgba(168,85,247,0.10),transparent_28%),radial-gradient(circle_at_60%_20%,rgba(34,211,238,0.06),transparent_22%)]">
          <header className="sticky top-0 z-30 border-b border-white/[0.05] bg-[#05070d]/85 px-6 py-4 backdrop-blur-2xl lg:px-8">
            <div className="flex items-center gap-4">
              <button
                type="button"
                onClick={handleSearchClick}
                className="relative max-w-xl flex-1 text-left"
                aria-label="Open Roggy search"
              >
                <Search className="absolute left-4 top-1/2 h-[18px] w-[18px] -translate-y-1/2 text-zinc-600" />

                <div className="flex h-11 w-full items-center rounded-2xl border border-white/[0.07] bg-white/[0.035] pl-11 pr-4 text-sm text-zinc-600 outline-none transition hover:border-purple-500/30 hover:bg-white/[0.05]">
                  Search songs, artists, albums...
                </div>
              </button>

              <button
                type="button"
                className="hidden rounded-xl border border-white/[0.06] bg-white/[0.03] p-2.5 text-zinc-500 transition hover:bg-white/[0.06] hover:text-white sm:block"
                aria-label="Notifications"
              >
                <Bell className="h-[18px] w-[18px]" />
              </button>

              <Link
                href="/profile"
                className="flex items-center gap-2 rounded-full border border-white/[0.07] bg-white/[0.035] py-1.5 pl-1.5 pr-3 transition hover:bg-white/[0.06]"
              >
                <div className="flex h-8 w-8 items-center justify-center rounded-full bg-gradient-to-br from-cyan-400 via-purple-500 to-orange-500 text-xs font-black">
                  R
                </div>

                <span className="hidden text-xs font-medium text-zinc-300 md:block">
                  Roggy User
                </span>

                <ChevronDown className="hidden h-3.5 w-3.5 text-zinc-600 md:block" />
              </Link>
            </div>
          </header>

          <div className="px-6 pb-32 pt-7 lg:px-8">
            {apiError && (
              <div className="mb-5 rounded-xl border border-orange-500/20 bg-orange-500/[0.06] px-4 py-3 text-xs text-orange-300">
                API connection issue: {apiError}
              </div>
            )}

            <section className="relative min-h-[330px] overflow-hidden rounded-[28px] border border-white/[0.07] bg-gradient-to-br from-[#17102b] via-[#101326] to-[#08131d] p-7 shadow-2xl shadow-purple-950/20 md:p-9">
              <div className="absolute -right-20 -top-28 h-80 w-80 rounded-full bg-fuchsia-500/20 blur-[90px]" />
              <div className="absolute -bottom-32 left-1/3 h-72 w-72 rounded-full bg-cyan-400/15 blur-[90px]" />
              <div className="absolute right-[18%] top-1/2 h-40 w-40 rounded-full bg-orange-500/10 blur-[70px]" />

              <div className="relative z-10 flex h-full flex-col justify-center">
                <div className="mb-4 flex items-center gap-2">
                  <span className="rounded-full border border-fuchsia-400/20 bg-fuchsia-500/10 px-3 py-1 text-[10px] font-semibold uppercase tracking-[0.18em] text-fuchsia-300">
                    Featured
                  </span>

                  {isLoading && (
                    <span className="text-[10px] uppercase tracking-[0.15em] text-zinc-600">
                      Loading music...
                    </span>
                  )}
                </div>

                <p className="mb-2 text-sm font-medium text-cyan-300">
                  Welcome back to Roggy
                </p>

                <h1 className="max-w-2xl text-4xl font-black tracking-tight text-white md:text-5xl">
                  Your music.
                  <br />
                  <span className="bg-gradient-to-r from-cyan-300 via-fuchsia-400 to-orange-300 bg-clip-text text-transparent">
                    Your world.
                  </span>
                </h1>

                <p className="mt-4 max-w-lg text-sm leading-6 text-zinc-400">
                  Discover fresh sounds, follow your favorite artists and
                  build a library that feels completely yours.
                </p>

                <div className="mt-7 flex flex-wrap gap-3">
                  <button
                    type="button"
                    onClick={() => handlePlay(featuredSong)}
                    className="flex items-center gap-2 rounded-full bg-gradient-to-r from-fuchsia-500 to-purple-600 px-5 py-3 text-xs font-bold text-white shadow-xl shadow-purple-600/20 transition hover:scale-[1.03]"
                  >
                    {currentSong?.id === featuredSong.id && isPlaying ? (
                      <Pause className="h-4 w-4 fill-current" />
                    ) : (
                      <Play className="h-4 w-4 fill-current" />
                    )}

                    {currentSong?.id === featuredSong.id && isPlaying
                      ? "Pause featured"
                      : "Play featured"}
                  </button>

                  <button
                    type="button"
                    onClick={handleShuffle}
                    className="flex items-center gap-2 rounded-full border border-white/10 bg-white/[0.05] px-5 py-3 text-xs font-semibold text-zinc-300 transition hover:bg-white/[0.08] hover:text-white"
                  >
                    <Shuffle className="h-4 w-4" />
                    Shuffle
                  </button>
                </div>

                <div className="mt-7 flex items-center gap-3 text-xs text-zinc-500">
                  <Clock3 className="h-4 w-4 text-cyan-400" />

                  <span>
                    Now featuring{" "}
                    <span className="font-medium text-zinc-300">
                      {featuredSong.title}
                    </span>
                  </span>
                </div>
              </div>

              <div className="absolute bottom-8 right-8 hidden w-48 md:block lg:right-14 lg:w-56">
                <div className="relative">
                  <div className="absolute inset-0 scale-90 rounded-[28px] bg-gradient-to-br from-cyan-400/30 via-purple-500/30 to-orange-500/30 blur-2xl" />

                  <div className="relative rotate-3 overflow-hidden rounded-[28px] border border-white/10 shadow-2xl">
                    <SongArtwork
                      song={featuredSong}
                      index={0}
                      size="large"
                    />
                  </div>
                </div>
              </div>
            </section>

            <section className="mt-9">
              <SectionHeader
                title="Recently Released"
                subtitle="Fresh music from your Roggy catalog"
                icon={<Clock3 className="h-5 w-5 text-cyan-400" />}
                href="/search"
              />

              <div className="grid grid-cols-2 gap-4 md:grid-cols-3 xl:grid-cols-5">
                {displayedRecentSongs.slice(0, 5).map((song, index) => (
                  <article
                    key={song.id}
                    className="group min-w-0 rounded-2xl border border-white/[0.06] bg-white/[0.025] p-3 transition duration-300 hover:-translate-y-1 hover:border-white/[0.11] hover:bg-white/[0.045]"
                  >
                    <div className="relative">
                      <SongArtwork
                        song={song}
                        index={index}
                        size="large"
                      />

                      <button
                        type="button"
                        onClick={() => handlePlay(song)}
                        className="absolute bottom-3 right-3 flex h-10 w-10 translate-y-2 items-center justify-center rounded-full bg-white text-black opacity-0 shadow-xl transition-all duration-300 group-hover:translate-y-0 group-hover:opacity-100"
                        aria-label={`Play ${song.title}`}
                      >
                        {currentSong?.id === song.id && isPlaying ? (
                          <Pause className="h-4 w-4 fill-current" />
                        ) : (
                          <Play className="h-4 w-4 fill-current" />
                        )}
                      </button>
                    </div>

                    <div className="px-1 pb-1 pt-3">
                      <h3 className="truncate text-sm font-semibold text-zinc-200">
                        {song.title}
                      </h3>

                      <p className="mt-1 truncate text-[11px] text-zinc-600">
                        Artist · {formatReleaseDate(song.releaseDate)}
                      </p>

                      <div className="mt-3 flex items-center justify-between">
                        <span className="text-[10px] text-zinc-600">
                          {formatDuration(song.duration)}
                        </span>

                        <span className="flex items-center gap-1 text-[10px] text-zinc-600">
                          <Play className="h-3 w-3" />
                          {song.playCount}
                        </span>
                      </div>
                    </div>
                  </article>
                ))}
              </div>
            </section>

            <section className="mt-10">
              <SectionHeader
                title="Trending Now"
                subtitle="What is getting attention on Roggy"
                icon={<Flame className="h-5 w-5 text-orange-400" />}
                href="/search"
              />

              <div className="grid gap-3 lg:grid-cols-2">
                {displayedTrendingSongs.slice(0, 6).map((song, index) => (
                  <article
                    key={song.id}
                    className="group flex items-center gap-4 rounded-2xl border border-white/[0.06] bg-white/[0.025] p-3 transition hover:border-white/[0.1] hover:bg-white/[0.045]"
                  >
                    <span className="w-5 text-center text-xs font-bold text-zinc-700">
                      {String(index + 1).padStart(2, "0")}
                    </span>

                    <SongArtwork
                      song={song}
                      index={index + 2}
                      size="small"
                    />

                    <div className="min-w-0 flex-1">
                      <h3 className="truncate text-sm font-semibold text-zinc-200">
                        {song.title}
                      </h3>

                      <p className="mt-1 truncate text-[11px] text-zinc-600">
                        Artist · {song.description || "Roggy release"}
                      </p>
                    </div>

                    <div className="hidden items-center gap-2 sm:flex">
                      <span className="text-[10px] text-zinc-600">
                        {formatDuration(song.duration)}
                      </span>

                      <button
                        type="button"
                        onClick={() => handlePlay(song)}
                        className="flex h-9 w-9 items-center justify-center rounded-full bg-white/[0.05] text-zinc-400 opacity-0 transition group-hover:opacity-100 hover:bg-purple-500/20 hover:text-purple-300"
                        aria-label={`Play ${song.title}`}
                      >
                        {currentSong?.id === song.id && isPlaying ? (
                          <Pause className="h-3.5 w-3.5 fill-current" />
                        ) : (
                          <Play className="h-3.5 w-3.5 fill-current" />
                        )}
                      </button>

                      <button
                        type="button"
                        className="rounded-lg p-2 text-zinc-700 transition hover:bg-white/[0.05] hover:text-zinc-300"
                        aria-label={`More options for ${song.title}`}
                      >
                        <MoreHorizontal className="h-4 w-4" />
                      </button>
                    </div>
                  </article>
                ))}
              </div>
            </section>

            <section className="mt-10">
              <SectionHeader
                title="Made for You"
                subtitle="Recommendations based on your listening"
                href="/search"
              />

              <div className="grid grid-cols-2 gap-4 md:grid-cols-4">
                {displayedRecommendedSongs.slice(0, 4).map((song, index) => (
                  <article
                    key={song.id}
                    className="group rounded-2xl border border-white/[0.06] bg-gradient-to-br from-white/[0.035] to-white/[0.015] p-3 transition hover:border-purple-500/20 hover:bg-white/[0.05]"
                  >
                    <div className="relative">
                      <SongArtwork
                        song={song}
                        index={index + 4}
                        size="large"
                      />

                      <button
                        type="button"
                        onClick={() => handlePlay(song)}
                        className="absolute bottom-3 right-3 flex h-10 w-10 translate-y-2 items-center justify-center rounded-full bg-white text-black opacity-0 shadow-xl transition-all group-hover:translate-y-0 group-hover:opacity-100"
                        aria-label={`Play ${song.title}`}
                      >
                        {currentSong?.id === song.id && isPlaying ? (
                          <Pause className="h-4 w-4 fill-current" />
                        ) : (
                          <Play className="h-4 w-4 fill-current" />
                        )}
                      </button>
                    </div>

                    <div className="pt-3">
                      <h3 className="truncate text-sm font-semibold text-zinc-200">
                        {song.title}
                      </h3>

                      <p className="mt-1 truncate text-[11px] text-zinc-600">
                        Recommended for you
                      </p>
                    </div>
                  </article>
                ))}
              </div>
            </section>

            <section className="mt-10 grid gap-5 lg:grid-cols-[1.4fr_0.8fr]">
              <div className="rounded-3xl border border-white/[0.06] bg-gradient-to-br from-purple-950/50 via-[#111022] to-[#080d16] p-6">
                <div className="flex items-start justify-between">
                  <div>
                    <span className="text-[10px] font-semibold uppercase tracking-[0.2em] text-fuchsia-400">
                      Your listening
                    </span>

                    <h2 className="mt-2 text-2xl font-bold text-white">
                      Keep discovering.
                    </h2>

                    <p className="mt-2 max-w-md text-xs leading-5 text-zinc-500">
                      Save songs you love, create playlists and keep building
                      your personal Roggy library.
                    </p>
                  </div>

                  <div className="hidden h-14 w-14 items-center justify-center rounded-2xl bg-gradient-to-br from-fuchsia-500 to-purple-600 shadow-xl shadow-purple-500/20 sm:flex">
                    <Heart className="h-6 w-6 fill-white text-white" />
                  </div>
                </div>

                <div className="mt-6 flex flex-wrap gap-3">
                  <Link
                    href="/library"
                    className="rounded-full bg-white px-4 py-2.5 text-xs font-bold text-black transition hover:scale-105"
                  >
                    Open Library
                  </Link>

                  <Link
                    href="/search"
                    className="rounded-full border border-white/10 bg-white/[0.04] px-4 py-2.5 text-xs font-semibold text-zinc-300 transition hover:bg-white/[0.08] hover:text-white"
                  >
                    Browse Artists
                  </Link>
                </div>
              </div>

              <div className="rounded-3xl border border-white/[0.06] bg-white/[0.025] p-6">
                <div className="flex items-center justify-between">
                  <div>
                    <p className="text-[10px] font-semibold uppercase tracking-[0.2em] text-zinc-600">
                      Quick action
                    </p>

                    <h3 className="mt-2 text-lg font-bold text-white">
                      Your music hub
                    </h3>
                  </div>

                  <div className="flex h-11 w-11 items-center justify-center rounded-xl bg-gradient-to-br from-cyan-400 to-blue-600 shadow-lg shadow-cyan-500/10">
                    <Search className="h-5 w-5 text-white" />
                  </div>
                </div>

                <div className="mt-5 grid grid-cols-2 gap-2">
                  <Link
                    href="/search"
                    className="rounded-xl border border-white/[0.05] bg-white/[0.025] px-3 py-3 text-left text-[11px] font-medium text-zinc-500 transition hover:bg-white/[0.06] hover:text-white"
                  >
                    Search
                  </Link>

                  <Link
                    href="/playlists"
                    className="rounded-xl border border-white/[0.05] bg-white/[0.025] px-3 py-3 text-left text-[11px] font-medium text-zinc-500 transition hover:bg-white/[0.06] hover:text-white"
                  >
                    Playlists
                  </Link>

                  <Link
                    href="/liked"
                    className="rounded-xl border border-white/[0.05] bg-white/[0.025] px-3 py-3 text-left text-[11px] font-medium text-zinc-500 transition hover:bg-white/[0.06] hover:text-white"
                  >
                    Liked Songs
                  </Link>

                  <Link
                    href="/downloads"
                    className="rounded-xl border border-white/[0.05] bg-white/[0.025] px-3 py-3 text-left text-[11px] font-medium text-zinc-500 transition hover:bg-white/[0.06] hover:text-white"
                  >
                    Downloads
                  </Link>
                </div>
              </div>
            </section>
          </div>
        </div>
      </div>

      <div className="fixed bottom-0 left-[270px] right-0 z-50 border-t border-white/[0.07] bg-[#080a11]/95 px-4 py-3 backdrop-blur-2xl lg:px-6">
        <div className="mx-auto flex max-w-[1600px] items-center gap-4">
          <div className="flex min-w-0 flex-1 items-center gap-3">
            {currentSong ? (
              <>
                <SongArtwork
                  song={{
                    ...currentSong,
                    albumId: null,
                    genreId: "",
                    description: null,
                    releaseDate: "",
                    contentRating: 1,
                    allowRemixing: false,
                    isPublished: true,
                    playCount: 0,
                    trackNumber: 1,
                    createdAt: "",
                    updatedAt: "",
                  }}
                  index={0}
                  size="small"
                />

                <div className="min-w-0">
                  <h3 className="truncate text-xs font-semibold text-zinc-200">
                    {currentSong.title}
                  </h3>

                  <p className="mt-0.5 truncate text-[10px] text-zinc-600">
                    Roggy Artist
                  </p>
                </div>

                <button
                  type="button"
                  className="hidden rounded-lg p-2 text-zinc-600 transition hover:bg-white/[0.05] hover:text-pink-400 sm:block"
                  aria-label="Like song"
                >
                  <Heart className="h-4 w-4" />
                </button>
              </>
            ) : (
              <div className="text-xs text-zinc-600">
                Select a song to start listening
              </div>
            )}
          </div>

          <div className="flex shrink-0 items-center gap-2">
            <button
              type="button"
              onClick={previous}
              disabled={!currentSong}
              className="hidden rounded-lg p-2 text-zinc-600 transition hover:text-white disabled:cursor-not-allowed disabled:opacity-30 md:block"
              aria-label="Previous song"
            >
              <SkipBack className="h-4 w-4 fill-current" />
            </button>

            <button
              type="button"
              onClick={handleTogglePlay}
              disabled={!currentSong}
              className="flex h-10 w-10 items-center justify-center rounded-full bg-white text-black shadow-lg transition hover:scale-105 disabled:cursor-not-allowed disabled:opacity-40"
              aria-label={isPlaying ? "Pause" : "Play"}
            >
              {isPlaying ? (
                <Pause className="h-4 w-4 fill-current" />
              ) : (
                <Play className="h-4 w-4 fill-current" />
              )}
            </button>

            <button
              type="button"
              onClick={next}
              disabled={!currentSong}
              className="hidden rounded-lg p-2 text-zinc-600 transition hover:text-white disabled:cursor-not-allowed disabled:opacity-30 md:block"
              aria-label="Next song"
            >
              <SkipForward className="h-4 w-4 fill-current" />
            </button>
          </div>

          <div className="hidden flex-1 items-center gap-3 lg:flex">
            <span className="text-[10px] text-zinc-600">
              {formatPlayerTime(currentTime)}
            </span>

            <input
              type="range"
              min={0}
              max={playerDuration || 1}
              value={Math.min(currentTime, playerDuration || 1)}
              onChange={(event) => seek(Number(event.target.value))}
              disabled={!currentSong}
              aria-label="Song progress"
              className="h-1 flex-1 cursor-pointer accent-fuchsia-500 disabled:cursor-not-allowed"
            />

            <span className="text-[10px] text-zinc-600">
              {formatPlayerTime(playerDuration)}
            </span>
          </div>

          <div className="hidden items-center gap-3 xl:flex">
            <Volume2 className="h-4 w-4 text-zinc-600" />

            <div className="h-1 w-20 overflow-hidden rounded-full bg-white/[0.08]">
              <div className="h-full w-3/4 rounded-full bg-zinc-500" />
            </div>

            <button
              type="button"
              className="rounded-lg p-2 text-zinc-600 transition hover:bg-white/[0.05] hover:text-white"
              aria-label="More player options"
            >
              <MoreHorizontal className="h-4 w-4" />
            </button>
          </div>
        </div>

        <div
          className="absolute bottom-0 left-0 h-[2px] bg-gradient-to-r from-cyan-400 via-purple-500 to-fuchsia-500 transition-all"
          style={{ width: `${progressPercentage}%` }}
        />
      </div>
    </main>
  );
}

function parseDurationToSeconds(duration: string) {
  const parts = duration.split(":").map(Number);

  if (parts.length !== 3 || parts.some(Number.isNaN)) {
    return 0;
  }

  return parts[0] * 3600 + parts[1] * 60 + parts[2];
}

function formatPlayerTime(seconds: number) {
  if (!Number.isFinite(seconds) || seconds < 0) {
    return "0:00";
  }

  const totalSeconds = Math.floor(seconds);
  const minutes = Math.floor(totalSeconds / 60);
  const remainingSeconds = totalSeconds % 60;

  return `${minutes}:${remainingSeconds.toString().padStart(2, "0")}`;
}