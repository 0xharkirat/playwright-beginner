export default function Home() {
  return (
    <div className="flex min-h-screen items-center justify-center bg-zinc-50 dark:bg-black">
      <main className="flex flex-col items-center gap-8 text-center">
        <h1 className="text-4xl font-bold text-black dark:text-white">
          Welcome Home
        </h1>
        <p className="text-lg text-zinc-600 dark:text-zinc-400">
          This is a simple home page.
        </p>
        <a
          href="/hello"
          className="rounded-full bg-black px-6 py-3 text-white transition-colors hover:bg-zinc-800 dark:bg-white dark:text-black dark:hover:bg-zinc-200"
        >
          Go to Hello Page
        </a>
      </main>
    </div>
  );
}
