#include "register_types.h"

#include "strobelight.h"

#include <gdextension_interface.h>
#include <godot_cpp/core/defs.hpp>
#include <godot_cpp/godot.hpp>
#include <godot_cpp/variant/utility_functions.hpp>

using namespace godot;

void initialize_strobelight_module(ModuleInitializationLevel p_level) {
	if (p_level != MODULE_INITIALIZATION_LEVEL_SCENE) {
		return;
	}

    // Debug
    UtilityFunctions::print("Registering StrobeLight class...");

    GDREGISTER_CLASS(StrobeLight);

    // Debug
    UtilityFunctions::print("StrobeLight class registered.");
}

void uninitialize_strobelight_module(ModuleInitializationLevel p_level) {
	if (p_level != MODULE_INITIALIZATION_LEVEL_SCENE) {
		return;
	}

    // Debug
    UtilityFunctions::print("Unregistering StrobeLight class.");
}

extern "C" {
    // Initialization.
    GDExtensionBool GDE_EXPORT strobelight_library_init(GDExtensionInterfaceGetProcAddress p_get_proc_address, const GDExtensionClassLibraryPtr p_library, GDExtensionInitialization *r_initialization) {
        godot::GDExtensionBinding::InitObject init_obj(p_get_proc_address, p_library, r_initialization);

        init_obj.register_initializer(initialize_strobelight_module);
        init_obj.register_terminator(uninitialize_strobelight_module);
        init_obj.set_minimum_library_initialization_level(MODULE_INITIALIZATION_LEVEL_SCENE);

        return init_obj.init();
    }
}