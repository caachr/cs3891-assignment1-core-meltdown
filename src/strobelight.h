#ifndef STROBE_LIGHT_H
#define STROBE_LIGHT_H

#include <godot_cpp/classes/node3d.hpp>
#include <godot_cpp/classes/mesh_instance3d.hpp>
#include <godot_cpp/classes/standard_material3d.hpp>
#include <godot_cpp/classes/light3d.hpp>
#include <godot_cpp/core/class_db.hpp>

namespace godot{
    class StrobeLight : public Node3D {
    GDCLASS(StrobeLight, Node3D)

    private:
        double frequency;       // The strobe's frequency of oscillation regarding the dimming and brightening cycle.
        double timeElapsed;     // The overall time elapsed since the start of _physics_process.
        double minIntensity;    // The intensity of light at the trough of the strobe's oscillation.
        double maxIntensity;    // The intensity of light at the peak of the strobe's oscillation.
        Light3D* light;         // The light node that will perform the strobe effect.

    protected:
        static void _bind_methods();

    public:
        StrobeLight();
        ~StrobeLight();

        void _ready() override;
        void _physics_process(double delta) override;
        
        // Getter for the frequency of oscillation.
        double get_frequency() const;

        // Setter for the frequency of oscillation.
        void set_frequency(const double newFreq);

        // Getter for the minimum intensity.
        double get_min_intensity() const;

        // Setter for the minimum intensity.
        void set_min_intensity(const double val);

        // Getter for the maximum intensity.
        double get_max_intensity() const;

        // Setter for the maximum intensity.
        void set_max_intensity(const double val);
    };
}

#endif