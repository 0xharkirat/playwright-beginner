"use client";

import { useEffect, useState } from "react";

export default function HelloPage() {
    const [message, setMessage] = useState<string>("");
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        async function fetchHello() {
            try {
                const response = await fetch("http://localhost:5278/api/Hello");
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                const data = await response.text();
                setMessage(data);
            } catch (err) {
                setError(err instanceof Error ? err.message : "Failed to fetch");
            } finally {
                setLoading(false);
            }
        }

        fetchHello();
    }, []);

    return (
        <div className="flex min-h-screen items-center justify-center bg-zinc-50 dark:bg-black">
            <main className="flex flex-col items-center gap-8 text-center">
                <h1 className="text-4xl font-bold text-black dark:text-white">
                    Hello Page
                </h1>

                {loading && (
                    <p className="text-lg text-zinc-600 dark:text-zinc-400">
                        Loading...
                    </p>
                )}

                {error && (
                    <p className="text-lg text-red-600 dark:text-red-400">
                        Error: {error}
                    </p>
                )}

                {message && (
                    <p className="text-2xl font-semibold text-black dark:text-white">
                        {message}
                    </p>
                )}

                <a
                    href="/"
                    className="rounded-full bg-black px-6 py-3 text-white transition-colors hover:bg-zinc-800 dark:bg-white dark:text-black dark:hover:bg-zinc-200"
                >
                    Back to Home
                </a>
            </main>
        </div>
    );
}
