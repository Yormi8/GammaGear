#include <pybind11/embed.h> // everything needed for embedding
#include <filesystem>       // Python needs to know where to find modules and libraries.
#include <iostream>         // Debugging
namespace py = pybind11;

int main(int argc, char** argv) {
    std::filesystem::path executable_path = argv[0];
    std::cout << "before wstring: " << executable_path.parent_path().c_str() << "\n";
    std::cout << "before wstring: " << executable_path << "\n";

    std::wstring prefix = executable_path.parent_path().c_str();
    prefix += L"\\vcpkg_installed\\x64-windows";

    std::wstring python_home = prefix + L"\\tools\\python3";

    std::array<std::wstring, 3> paths = {
        prefix + L"\\tools\\python3;",
        prefix + L"\\tools\\python3\\Lib;",
        prefix + L"\\tools\\python3\\DLLs"
    };

    std::wstring python_path = L"";
    for (size_t i = 0; i < paths.size(); i++)
    {
        python_path += paths[i];
    }

    Py_SetPath(python_path.c_str());
    Py_SetPythonHome(python_home.c_str());

    PyConfig config{};
    PyConfig_InitIsolatedConfig(&config);
    PyConfig_SetString(&config, &config.home, python_home.c_str());
    PyConfig_SetString(&config, &config.pythonpath_env, python_path.c_str());

    py::scoped_interpreter guard(&config); // start the interpreter and keep it alive

    py::print("Hello, World!"); // use the Python API
}