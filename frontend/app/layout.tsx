import type { Metadata } from "next";
import { Geist, Geist_Mono } from "next/font/google";

import "./globals.css";
import AuthGuard from "../Components/AuthGuard";
import { AudioPlayerProvider } from "../Components/AudioPlayerProvider";

const geistSans = Geist({
  variable: "--font-geist-sans",
  subsets: ["latin"],
});

const geistMono = Geist_Mono({
  variable: "--font-geist-mono",
  subsets: ["latin"],
});

export const metadata: Metadata = {
  title: "Roggy",
  description: "Your music. Your world.",
};

export default function RootLayout({ children }: LayoutProps<"/">) {
  return (
    <html
      lang="en"
      className={`${geistSans.variable} ${geistMono.variable} h-full antialiased`}
    >
      <body className="min-h-full flex flex-col">
        <AudioPlayerProvider>
          <AuthGuard>{children}</AuthGuard>
        </AudioPlayerProvider>
      </body>
    </html>
  );
}