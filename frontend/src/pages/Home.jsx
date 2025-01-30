import { useState } from "react";
import axios from "axios";
import { PaperAirplaneIcon } from "@heroicons/react/24/solid";
import Navbar from "../components/Navbar";
import Footer from "../components/Footer";

const Home = () => {
    const [email, setEmail] = useState("");
    const [destination, setDestination] = useState("");
    const [startDate, setStartDate] = useState("");
    const [endDate, setEndDate] = useState("");
    const [hotels, setHotels] = useState([]);
    const [loading, setLoading] = useState(false);
    const [showResults, setShowResults] = useState(false);

    const handleSearch = async () => {
        if (!email || !destination || !startDate || !endDate) {
            alert("Please fill all fields.");
            return;
        }

        setLoading(true);

        try {
            const response = await axios.get(
                "http://localhost:5000/api/hotels/search",
                {
                    params: {
                        location: destination,
                        checkIn: startDate,
                        checkOut: endDate,
                    },
                }
            );

            setHotels(response.data.hotels);
            setShowResults(true);
        } catch (error) {
            console.error("Error fetching hotels:", error);
            alert("Failed to fetch hotel data.");
        }

        setLoading(false);
    };

    return (
        <div className="min-h-screen flex flex-col bg-gray-100">
            <Navbar />
            <main className="flex-1 flex flex-col items-center justify-center text-center p-6">
                <h1 className="text-5xl font-bold text-blue-600 mb-6 flex items-center">
                    Plan Your Dream Trip <PaperAirplaneIcon className="h-8 w-8 ml-2 text-blue-500" />
                </h1>

                {/* Input Form */}
                {!showResults && (
                    <div className="bg-white p-6 rounded-lg shadow-lg w-full max-w-lg">
                        <input type="email" placeholder="Enter your email" className="w-full p-3 border rounded-lg mb-4" value={email} onChange={(e) => setEmail(e.target.value)} />
                        <input type="text" placeholder="Destination (e.g., New York)" className="w-full p-3 border rounded-lg mb-4" value={destination} onChange={(e) => setDestination(e.target.value)} />
                        <div className="flex space-x-4">
                            <input type="date" className="w-full p-3 border rounded-lg" value={startDate} onChange={(e) => setStartDate(e.target.value)} />
                            <input type="date" className="w-full p-3 border rounded-lg" value={endDate} onChange={(e) => setEndDate(e.target.value)} />
                        </div>
                        <button onClick={handleSearch} className="w-full bg-blue-600 text-white font-semibold py-3 rounded-lg mt-4 hover:bg-blue-700 transition duration-300">
                            {loading ? "Searching..." : "Plan My Trip"}
                        </button>
                    </div>
                )}

                {/* Hotel Listings */}
                {showResults && (
                    <div className="mt-10 w-full max-w-5xl">
                        <h2 className="text-3xl font-bold text-blue-600 mb-4">Recommended Hotels</h2>
                        <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                            {hotels.map((hotel) => (
                                <div key={hotel.id} className="bg-white shadow-lg rounded-lg overflow-hidden">
                                    <img src={hotel.image || "https://source.unsplash.com/400x250/?hotel"} alt={hotel.name} className="w-full h-40 object-cover" />
                                    <div className="p-4">
                                        <h3 className="text-xl font-bold">{hotel.name}</h3>
                                        <p className="text-gray-600">Total Stay: <strong>${hotel.pricePerNight * 3}</strong></p>
                                        <p className="text-sm text-gray-500">Per Night: ${hotel.pricePerNight}</p>
                                        <p className="text-green-600 font-semibold">{hotel.discount || "No Discount"}</p>
                                    </div>
                                </div>
                            ))}
                        </div>
                    </div>
                )}
            </main>
            <Footer />
        </div>
    );
};

export default Home;
