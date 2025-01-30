import { Link } from "react-router-dom";

const Navbar = () => {
    return (
        <nav className="bg-blue-600 text-white py-4 shadow-md">
            <div className="container mx-auto flex justify-between items-center px-6">
                <Link to="/" className="text-2xl font-bold">
                    Travel Planner
                </Link>
                <div className="space-x-6">
                    <Link to="/" className="hover:underline">Home</Link>
                    <Link to="/destinations" className="hover:underline">Destinations</Link>
                </div>
            </div>
        </nav>
    );
};

export default Navbar;
