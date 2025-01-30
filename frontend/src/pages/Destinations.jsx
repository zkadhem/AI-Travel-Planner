import Navbar from "../components/Navbar";
import Footer from "../components/Footer";

const Destinations = () => {
    return (
        <div className="min-h-screen flex flex-col">
            <Navbar />
            <main className="flex-1 flex flex-col items-center justify-center text-center">
                <h1 className="text-4xl font-bold text-blue-600">Top Destinations</h1>
                <p className="mt-4 text-lg text-gray-600">Discover amazing places to visit.</p>
            </main>
            <Footer />
        </div>
    );
};

export default Destinations;
