"use client";

import {
    createContext,
    ReactNode,
    useContext,
    useEffect,
    useRef,
    useState,
} from "react";

export type PlayerSong = {
    id: string;
    title: string;
    artistId: string;
    audioUrl: string;
    coverImageUrl: string | null;
    duration: string;
};

type AudioPlayerContextValue = {
    currentSong: PlayerSong | null;
    isPlaying: boolean;
    currentTime: number;
    duration: number;
    playSong: (song: PlayerSong) => void;
    togglePlay: () => void;
    pause: () => void;
    resume: () => void;
    seek: (time: number) => void;
    next: () => void;
    previous: () => void;
};

const AudioPlayerContext =
    createContext<AudioPlayerContextValue | null>(null);

export function AudioPlayerProvider({
    children,
}: {
    children: ReactNode;
}) {
    const audioRef = useRef<HTMLAudioElement | null>(null);

    const [currentSong, setCurrentSong] =
        useState<PlayerSong | null>(null);

    const [isPlaying, setIsPlaying] = useState(false);
    const [currentTime, setCurrentTime] = useState(0);
    const [duration, setDuration] = useState(0);

    useEffect(() => {
        const audio = new Audio();

        audio.preload = "metadata";
        audioRef.current = audio;

        function handleTimeUpdate() {
            setCurrentTime(audio.currentTime);
        }

        function handleLoadedMetadata() {
            setDuration(audio.duration);
        }

        function handleEnded() {
            setIsPlaying(false);
            setCurrentTime(0);
        }

        audio.addEventListener("timeupdate", handleTimeUpdate);
        audio.addEventListener("loadedmetadata", handleLoadedMetadata);
        audio.addEventListener("ended", handleEnded);

        return () => {
            audio.pause();

            audio.removeEventListener("timeupdate", handleTimeUpdate);
            audio.removeEventListener(
                "loadedmetadata",
                handleLoadedMetadata,
            );
            audio.removeEventListener("ended", handleEnded);

            audioRef.current = null;
        };
    }, []);

    function playSong(song: PlayerSong) {
        const audio = audioRef.current;

        if (!audio) {
            return;
        }

        if (!song.audioUrl) {
            console.warn("This song does not have a playable audio URL.");
            return;
        }

        if (currentSong?.id !== song.id) {
            audio.src = song.audioUrl;
            audio.currentTime = 0;

            setCurrentSong(song);
            setCurrentTime(0);
            setDuration(0);
        }

        audio
            .play()
            .then(() => {
                setIsPlaying(true);
            })
            .catch((error) => {
                console.error("Roggy audio playback failed:", error);
                setIsPlaying(false);
            });
    }

    function togglePlay() {
        const audio = audioRef.current;

        if (!audio || !currentSong) {
            return;
        }

        if (audio.paused) {
            audio
                .play()
                .then(() => {
                    setIsPlaying(true);
                })
                .catch((error) => {
                    console.error("Roggy audio playback failed:", error);
                    setIsPlaying(false);
                });

            return;
        }

        audio.pause();
        setIsPlaying(false);
    }

    function pause() {
        const audio = audioRef.current;

        if (!audio) {
            return;
        }

        audio.pause();
        setIsPlaying(false);
    }

    function resume() {
        const audio = audioRef.current;

        if (!audio || !currentSong) {
            return;
        }

        audio
            .play()
            .then(() => {
                setIsPlaying(true);
            })
            .catch((error) => {
                console.error("Roggy audio playback failed:", error);
                setIsPlaying(false);
            });
    }

    function seek(time: number) {
        const audio = audioRef.current;

        if (!audio) {
            return;
        }

        audio.currentTime = time;
        setCurrentTime(time);
    }

    function next() {
        console.log("Roggy next-track handling will be connected later.");
    }

    function previous() {
        console.log(
            "Roggy previous-track handling will be connected later.",
        );
    }

    return (
        <AudioPlayerContext.Provider
            value={{
                currentSong,
                isPlaying,
                currentTime,
                duration,
                playSong,
                togglePlay,
                pause,
                resume,
                seek,
                next,
                previous,
            }}
        >
            {children}
        </AudioPlayerContext.Provider>
    );
}

export function useAudioPlayer() {
    const context = useContext(AudioPlayerContext);

    if (!context) {
        throw new Error(
            "useAudioPlayer must be used inside AudioPlayerProvider.",
        );
    }

    return context;
}