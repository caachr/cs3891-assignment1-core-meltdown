#include "strobelight.h"
#include <godot_cpp/core/class_db.hpp>
#include <godot_cpp/variant/utility_functions.hpp>
#include <godot_cpp/classes/light3d.hpp>
#include <cmath>

using namespace godot;

void StrobeLight::_bind_methods() {
    // Debug
    // UtilityFunctions::print("StrobeLight class binding");

    ClassDB::bind_method(D_METHOD("get_frequency"), &StrobeLight::get_frequency);
    ClassDB::bind_method(D_METHOD("set_frequency", "frequency"), &StrobeLight::set_frequency);
    ClassDB::bind_method(D_METHOD("get_min_intensity"), &StrobeLight::get_min_intensity);
    ClassDB::bind_method(D_METHOD("set_min_intensity", "intensity"), &StrobeLight::set_min_intensity);
    ClassDB::bind_method(D_METHOD("get_max_intensity"), &StrobeLight::get_max_intensity);
    ClassDB::bind_method(D_METHOD("set_max_intensity", "intensity"), &StrobeLight::set_max_intensity);

    ADD_PROPERTY(PropertyInfo(Variant::FLOAT, "frequency", PROPERTY_HINT_RANGE, "0.1,10,0.1"), "set_frequency", "get_frequency");
    ADD_PROPERTY(PropertyInfo(Variant::FLOAT, "min_intensity", PROPERTY_HINT_RANGE, "0,10,0.1"), "set_min_intensity", "get_min_intensity");
    ADD_PROPERTY(PropertyInfo(Variant::FLOAT, "max_intensity", PROPERTY_HINT_RANGE, "0,10,0.1"), "set_max_intensity", "get_max_intensity");
}

double StrobeLight::get_frequency() const { return frequency; }
void StrobeLight::set_frequency(const double newFreq) { frequency = newFreq; }
double StrobeLight::get_min_intensity() const { return minIntensity; }
void StrobeLight::set_min_intensity(const double val) { minIntensity = val; }
double StrobeLight::get_max_intensity() const { return maxIntensity; }
void StrobeLight::set_max_intensity(const double val) { maxIntensity = val; }

StrobeLight::StrobeLight() {
    // Debug
    // UtilityFunctions::print("StrobeLight constructor called");

    frequency = 0.25;
    timeElapsed = 0.0;
    minIntensity = 0.0;
    maxIntensity = 40.0;
    light = nullptr;
}

StrobeLight::~StrobeLight() {
    // Debug
    // UtilityFunctions::print("StrobeLight destructor called");
}

void StrobeLight::_ready() {
    // Cast the node to Light3D
    light = Object::cast_to<Light3D>(get_node_or_null("AlarmStrobe"));
    if (!light) {
        UtilityFunctions::print("AlarmStrobe is not a Light3D!");
        return;
    }
}

void StrobeLight::_physics_process(double delta) {
    // Update the elapsed time
    timeElapsed += delta;

    // Calculate the intensity using a sine wave
    double intensity = minIntensity + (maxIntensity - minIntensity) * 0.5 * (1.0 + std::sin(2.0 * Math_PI * frequency * timeElapsed));

    // Set the light's intensity
    if (light) {
        light->set_param(Light3D::PARAM_ENERGY, intensity);
        
        // Debug
        // UtilityFunctions::print("Output intensity: ", light->get_param(Light3D::PARAM_ENERGY));
    } else {
        UtilityFunctions::print("Mesh is not a Light3D!");
    }
}