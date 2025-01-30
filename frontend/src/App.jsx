function App() {
    return (
        <div className="h-screen flex flex-col items-center justify-center bg-gradient-to-r from-purple-500 to-blue-500 text-white">
            <h1 className="text-5xl font-bold animate-bounce">?? Tailwind is Working! ??</h1>
            <p className="mt-4 text-lg">If you see this with a gradient background, Tailwind is set up correctly.</p>
            <button className="mt-6 px-6 py-3 bg-white text-blue-600 font-semibold rounded-lg shadow-lg hover:bg-gray-200 transition duration-300">
                Click Me!
            </button>
        </div>
    );
}

export default App;
