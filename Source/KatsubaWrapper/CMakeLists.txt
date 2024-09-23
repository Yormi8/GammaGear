cmake_minimum_required(VERSION 3.15)

project(katsuba_wrapper LANGUAGES CXX)

find_package(Python COMPONENTS Interpreter Development)
find_package(pybind11 CONFIG)

message("Python_FOUND: ${Python_FOUND}")
message("Python_EXECUTABLE: ${Python_EXECUTABLE}")
message("Python_VERSION: ${Python_VERSION}")
message("Python_Development.Embed_FOUND: ${Python_Development.Embed_FOUND}")
message("Python_INCLUDE_DIRS: ${Python_INCLUDE_DIRS}")
message("Python_LIBRARIES: ${Python_LIBRARIES}")
message("Python_LIBRARY_DIRS: ${Python_LIBRARY_DIRS}")
message("Python_RUNTIME_LIBRARY_DIRS: ${Python_RUNTIME_LIBRARY_DIRS}")

#add_library(katsuba_wrapper SHARED
#    "katsuba_wrapper.hpp"
#    "katsuba_wrapper.cpp"
#)

add_executable(katsuba_wrapper
"main.cpp"
)

target_link_libraries(katsuba_wrapper PRIVATE pybind11::embed)
target_link_libraries(katsuba_wrapper PRIVATE Python::Python)

target_compile_features(katsuba_wrapper PRIVATE cxx_std_17)