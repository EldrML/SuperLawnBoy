extends Area2D

var animation_speed = 5
var moving = false

var tile_size = 16
var inputs = {"right": Vector2.RIGHT,
			"left": Vector2.LEFT,
			"up": Vector2.UP,
			"down": Vector2.DOWN,}
var pressed_inputs = []
@onready var ray = $RayCast2D
@onready var anim_player = $Lawnboy_AnimationPlayer

# Called when the node enters the scene tree for the first time.
func _ready():
	position = position.snapped(Vector2.ONE * tile_size)
	position += Vector2.ONE * tile_size/2
	anim_player.play("face_down")


#DOCSTRING TODO
func move(dir):
	ray.target_position = inputs[dir] * tile_size
	ray.force_raycast_update()
	if !ray.is_colliding():
		var tween = create_tween()
		tween.tween_property(self, 
							"position", position + inputs[dir]*tile_size, 
							1.0/animation_speed).set_trans(Tween.TRANS_LINEAR)
		tween.tween_callback(func(): moving = false)

#DOCSTRING TODO
func input_processor():
	for dir in inputs.keys():
		if Input.is_action_just_pressed(dir):
			pressed_inputs.append(dir)
		if Input.is_action_just_released(dir):
			pressed_inputs.erase(dir)
		#TODO: set up-down or left-right to cancel each other out!
		

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	input_processor()
	
	# Movement processing
	if !pressed_inputs.is_empty() and moving == false:
		moving = true
		var anim_str = "walk_" + pressed_inputs.back()
		anim_player.play(anim_str)
		move(pressed_inputs.back())
