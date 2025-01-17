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
        double frequency;
        double timeElapsed;
        double minIntensity;
        double maxIntensity;
        Ref<StandardMaterial3D> material;
        Light3D* light;

    protected:
        static void _bind_methods();

    public:
        StrobeLight();
        ~StrobeLight();

        void _ready() override;
        void _physics_process(double delta) override;
        
        double get_frequency() const;
        void set_frequency(const double newFreq);

        double get_min_intensity() const;
        void set_min_intensity(const double val);
        double get_max_intensity() const;
        void set_max_intensity(const double val);
    };
}

#endif